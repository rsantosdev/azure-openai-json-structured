using OpenAI.Chat;

namespace WebApplication1.Helpers;

public static class CountryInfo
{
    public static ChatCompletionOptions Options = new ChatCompletionOptions
    {
        ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
            "country_info",
            jsonSchema: BinaryData.FromObjectAsJson(new
            {
                type = "object",
                properties = new
                {
                    name = new { type = "string" },
                    twoLetterIsoCode = new { type = "string" },
                    area = new { type = "number" },
                    capital = new { type = "string" },
                    habitants = new { type = "number" }
                }
            })
        )
    };
}

public class CountryInfoModel
{
    public string Name { get; set; }
    public string TwoLetterIsoCode { get; set; }
    public decimal Area { get; set; }
    public string Capital { get; set; }
    public decimal Habitants { get; set; }
}