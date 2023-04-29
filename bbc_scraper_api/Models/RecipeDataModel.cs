using System.Text.Json.Serialization;

namespace bbc_scraper_api.Models
{
    public class ContentAPIResponse
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("showTitleIcon")]
        public bool ShowTitleIcon { get; set; }

        [JsonPropertyName("showOverlayIcon")]
        public bool ShowOverlayIcon { get; set; }

        [JsonPropertyName("contentType")]
        public object ContentType { get; set; }

        [JsonPropertyName("theme")]
        public object Theme { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("label")]
        public object Label { get; set; }

        [JsonPropertyName("kicker")]
        public string Kicker { get; set; }

        [JsonPropertyName("postFormat")]
        public string PostFormat { get; set; }

        [JsonPropertyName("rating")]
        public ContentAPRating Rating { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("isPremium")]
        public bool IsPremium { get; set; }

        [JsonPropertyName("subscriptionExperience")]
        public string SubscriptionExperience { get; set; }

        [JsonPropertyName("image")]
        public Image Image { get; set; }

        [JsonPropertyName("postType")]
        public string PostType { get; set; }

        [JsonPropertyName("headingStyledSize")]
        public int HeadingStyledSize { get; set; }

        [JsonPropertyName("description")]
        public object Description { get; set; }
    }

    public class Image
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("alt")]
        public string Alt { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("aspectRatio")]
        public string AspectRatio { get; set; }
    }

    public class ContentAPRating
    {
        [JsonPropertyName("ratingValue")]
        public decimal RatingValue { get; set; }

        [JsonPropertyName("ratingCount")]
        public decimal RatingCount { get; set; }

        [JsonPropertyName("ratingTypeLabel")]
        public string RatingTypeLabel { get; set; }

        [JsonPropertyName("hasRatingCount")]
        public bool HasRatingCount { get; set; }

        [JsonPropertyName("isHalfStar")]
        public bool IsHalfStar { get; set; }
    }

}
