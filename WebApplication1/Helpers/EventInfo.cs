using OpenAI.Chat;

namespace WebApplication1.Helpers;

public class EventInfo
{
    public static ChatCompletionOptions Options = new ChatCompletionOptions
    {
        ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
            "event_info",
            jsonSchema: BinaryData.FromObjectAsJson(new
            {
                type = "object",
                properties = new
                {
                    name = new { type = "string" },
                    date = new { type = "string" },
                    participants = new { type = "array", items = new { type = "string" } }
                }
            })
        )
    };
}

public class EventInfoModel
{
    public string Name { get; set; }
    public string Date { get; set; }
    public string[] Participants { get; set; } = Array.Empty<string>();
}