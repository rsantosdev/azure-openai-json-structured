using System.Text.Json;
using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using WebApplication1.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<AzureOpenAIClient>(new AzureOpenAIClient(
    new Uri(builder.Configuration["AzureOpenAi:Endpoint"]),
    new AzureKeyCredential(builder.Configuration["AzureOpenAi:Key"])));

builder.Services.AddSingleton<ChatClient>(sp =>
{
    var azureAiClient = sp.GetRequiredService<AzureOpenAIClient>();
    return azureAiClient.GetChatClient(builder.Configuration["AzureOpenAi:Model"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapPost("/event", async (ChatRequest chatRequest, ChatClient chatClient) =>
{
    ChatCompletion completion = await chatClient.CompleteChatAsync(
    [
        new SystemChatMessage("Extract the event information."),
        new UserChatMessage(chatRequest.Message)
    ], options: EventInfo.Options);

    var eventInfo = JsonSerializer.Deserialize<EventInfoModel>(completion.Content[0].Text, JsonSerializerOptions.Web);
    return eventInfo;
});

app.MapPost("/country", async (ChatRequest chatRequest, ChatClient chatClient) =>
{
    ChatCompletion completion = await chatClient.CompleteChatAsync(
    [
        new SystemChatMessage("Please tell me something about the following country:"),
        new UserChatMessage(chatRequest.Message)
    ]);

    return completion.Content[0].Text;
});

app.MapPost("/country-structured", async (ChatRequest chatRequest, ChatClient chatClient) =>
{
    ChatCompletion completion = await chatClient.CompleteChatAsync([chatRequest.Message], CountryInfo.Options);
    var countryInfo = JsonSerializer.Deserialize<CountryInfoModel>(completion.Content[0].Text, JsonSerializerOptions.Web);
    return countryInfo;
});

app.Run();

public class ChatRequest
{
    public string Message { get; set; } = string.Empty;
}
