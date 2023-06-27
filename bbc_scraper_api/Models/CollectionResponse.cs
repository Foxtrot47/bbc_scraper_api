using System.Text.Json.Serialization;

namespace bbc_scraper_api.Models;

public class CollectionResponse
{
    [JsonPropertyName("items")]
    public List<CollectionItem> Items { get; set; }
    
    [JsonPropertyName("nextUrl")] 
    public string NextUrl { get; set; } = null!;
}

public class  CollectionItem 
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    
    [JsonPropertyName("isPremium")]
    public bool IsPremium { get; set; }
    
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}