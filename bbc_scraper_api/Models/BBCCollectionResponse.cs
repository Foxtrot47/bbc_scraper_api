using System.Text.Json.Serialization;

namespace bbc_scraper_api.Models;

public class BbcCollectionApiResponseRoot
{
    [JsonPropertyName("items")]
    public List<BbcCollectionApiResponseItem>? Items { get; set; }

    [JsonPropertyName("nextUrl")] public string? NextUrl { get; set; }
}

public class BbcCollectionApiResponseItem
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("title")] public string? Name { get; set; }

    [JsonPropertyName("Image")] public Image? Image { get; set; }

    [JsonPropertyName("rating")] public ContentAPRating? Rating { get; set; }
}
