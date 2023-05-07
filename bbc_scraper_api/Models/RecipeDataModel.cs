using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace bbc_scraper_api.Models
{
    [BsonIgnoreExtraElements]
    public class RecipeDataModel
    {
        [BsonId]
        public int Id { get; set; }

        [BsonElement("category")]
        public List<string>? Category { get; set; }

        [BsonElement("cusine")]
        public List<string>? Cusine { get; set; }

        [BsonElement("date")]
        public string? Date { get; set; }

        [BsonElement("description")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [BsonElement("diet")]
        public List<RecipeDataDiet>? Diet { get; set; }

        [BsonElement("image")]
        public RecipeDataImage? Image { get; set; }

        [BsonElement("ingredients")]
        public List<RecipeDataIngredientsModel>? Ingredients { get; set; }

        [BsonElement("instructions")]
        public List<RecipeDataInstructions>? Instructions { get; set; }

        [BsonElement("keywords")]
        public List<string>? Keywords { get; set; }

        [BsonElement("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; } = null!;

        [BsonElement("nutritionalInfo")]
        [JsonPropertyName("nutritionalInfo")]
        public List<RecipeDataNutritionalInfo>? NutritionalInfo { get; set; }

        [BsonElement("rating")]
        [JsonPropertyName("rating")]
        public RecipeDataRating? Rating { get; set; }

        [BsonElement("slug")]
        [JsonPropertyName("slug")]
        public string? Slug { get; set; } = null!;

        [BsonElement("similiarRecipes")]
        [JsonPropertyName("similiarRecipes")]
        public List<SimiliarRecipeData>? SimiliarRecipes { get; set; }

        [BsonElement("skillLevel")]
        [JsonPropertyName("skillLevel")]
        public string? SkillLevel { get; set; }

        [BsonElement("time")]
        [JsonPropertyName("time")]
        public RecipeDataTime? Time { get; set; }

        [BsonElement("yield")]
        [JsonPropertyName("yield")]
        public string? Yield { get; set; }

    }
    [BsonIgnoreExtraElements]
    public class RecipeDataIngredientsModel
    {
        [BsonElement("heading")]
        public string? Heading { get; set; }

        //[BsonElement("@id")]
        //public string Id { get; set; }

        [BsonElement("ingredients")]
        public List<RecipeDataIngredientModel>? Ingredients { get; set; }

    }

    [BsonIgnoreExtraElements]
    public class RecipeDataIngredientModel
    {
        //[BsonElement("@id")]
        //public string Id { get; set; }

        [BsonElement("@type")]
        public string? Type { get; set; }

        [BsonElement("ingredientText")]
        public string? IngredientText { get; set; }

        [BsonElement("note")]
        public string? Note { get; set; }

        [BsonElement("quantityText")]
        public string? QuantityText { get; set; }

        [BsonElement("term")]
        public RecipeDataIngredientTerm? Term { get; set; }

    }

    [BsonIgnoreExtraElements]
    public class RecipeDataIngredientTerm
    {
        [BsonId]
        public string? Id { get; set; }

        [BsonElement("@type")]
        public string? Type { get; set; }

        [BsonElement("slug")]
        public string? Slug { get; set; }

        [BsonElement("display")]
        public string? Display { get; set; }

        [BsonElement("taxonomy")]
        public string? Taxonomy { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class RecipeDataInstructions
    {
        [BsonElement("@type")]
        public string? Type { get; set; }

        [BsonElement("text")]
        public string? Text { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class RecipeDataNutritionalInfo
    {
        [BsonElement("label")]
        public string? Label { get; set; }

        [BsonElement("prefix")]
        public string? Prefix { get; set; }

        [BsonElement("suffix")]
        public string? Suffix { get; set; }

        [BsonElement("value")]
        public double Value { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class RecipeDataImage
    {
        [BsonElement("url")]
        public string? Url { get; set; }

        [BsonElement("title")]
        public string? Title { get; set; }

        [BsonElement("alt")]
        public string? Alt { get; set; }

        [BsonElement("width")]
        public int Width { get; set; }

        [BsonElement("height")]
        public int Height { get; set; }

        [BsonElement("aspectRatio")]
        public double? AspectRatio { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class RecipeDataRating
    {
        [BsonElement("avg")]
        [JsonPropertyName("avg")]
        public float Average { get; set; }

        [BsonElement("isHalfStar")]
        [JsonPropertyName("isHalfStar")]
        public bool IsHalfStar { get; set; }

        [BsonElement("total")]
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class RecipeDataTime
    {
        [BsonElement("cookTime")]
        [JsonPropertyName("cookTime")]
        public int CookTime { get; set; }

        [BsonElement("prepTime")]
        [JsonPropertyName("prepTime")]
        public int PrepTime { get; set; }

        [BsonElement("totalTime")]
        [JsonPropertyName("totalTime")]
        public int TotalTime { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class SimiliarRecipeData
    {
        [BsonElement("title")]
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [BsonElement("url")]
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [BsonElement("image")]
        [JsonPropertyName("image")]
        public SimiliarRecipeDataImage? Image { get; set; }

        [BsonElement("rating")]
        [JsonPropertyName("rating")]
        public ContentAPRating? Rating { get; set; }
    }

    public class SimiliarRecipeDataImage
    {
        [BsonElement("url")]
        public string? Url { get; set; }

        [BsonElement("title")]
        public string? Title { get; set; }

        [BsonElement("alt")]
        public string? Alt { get; set; }

        [BsonElement("width")]
        public int Width { get; set; }

        [BsonElement("height")]
        public int Height { get; set; }

        [BsonElement("aspectRatio")]
        public string? AspectRatio { get; set; }
    }
    public class RecipeDataDiet
    {
        [BsonElement("slug")]
        public string? Slug { get; set; }

        [BsonElement("display")]
        public string? Display { get; set; }

        [BsonElement("taxonomy")]
        public string? Taxonomy { get; set; }
    }
}
