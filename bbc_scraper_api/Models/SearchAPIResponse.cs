using System.Text.Json;
using System.Text.Json.Serialization;

public class Item
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("postType")]
    public string PostType { get; set; }

    [JsonPropertyName("isPremium")]
    public bool IsPremium { get; set; }
}


public class SearchResult
{
    [JsonPropertyName("items")]
    public List<Item> Items { get; set; }

    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    [JsonPropertyName("totalItems")]
    public int TotalItems { get; set; }

}

public class Result
{
    [JsonPropertyName("searchResults")]
    public SearchResult SearchResults { get; set; }
}
