using System.Text.Json;
using System.Text.Json.Serialization;

namespace bbc_scraper_api.Models 
{
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

    public class ContentAPIRequest
    {
        public string siteKey { get; set; }
        public int postId { get; set; }
        public string searchTerm { get; set; }
        public int widgetLimit { get; set; }
        public List<string> type { get; set; }
        public bool showCardLabels { get; set; }
        public bool v5enabled { get; set; }
    }
}
