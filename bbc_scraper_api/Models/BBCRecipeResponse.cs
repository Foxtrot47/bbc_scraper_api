using System.Text.Json.Serialization;

namespace bbc_scraper_api.Models
{
    public class BBCRecipeResponse
    {
        [JsonPropertyName("props")]
        public Props? Props { get; set; }
    }
    public class Props
    {
        [JsonPropertyName("pageProps")]
        public PageProps? PageProps { get; set; }
    }

    public class PageProps
    {
        [JsonPropertyName("postId")] public string? PostId { get; set; }

        [JsonPropertyName("slug")] public string? Slug { get; set; }

        [JsonPropertyName("authors")] public List<Author>? Authors { get; set; }

        [JsonPropertyName("userRatings")] public UserRatings? UserRatings { get; set; }

        [JsonPropertyName("ingredients")] public List<Ingredient>? Ingredients { get; set; }

        [JsonPropertyName("nutritionalInfo")] public List<NutritionalInfo>? NutritionalInfo { get; set; }

        [JsonPropertyName("skillLevel")] public string? SkillLevel { get; set; }

        [JsonPropertyName("cookAndPrepTime")] public CookAndPrepTime? CookAndPrepTime { get; set; }

        [JsonPropertyName("schema")] public Schema? Schema { get; set; }

        [JsonPropertyName("image")] public RecipeImage? Image { get; set; }

        [JsonPropertyName("diet")] public List<Diet>? Diet { get; set; }
    }

    public class Author
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("avatar")]
        public Avatar? Avatar { get; set; }
    }
    public class Avatar
    {
        [JsonPropertyName("@id")]
        public string? Id { get; set; } = null!;

        [JsonPropertyName("@type")]
        public string? Type { get; set; } = null!;

        [JsonPropertyName("aspectRatio")]
        public decimal AspectRatio { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; } = 0;

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }
    }

    public class UserRatings
    {
        [JsonPropertyName("avg")]
        public float Avg { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("isHalfStar")]
        public bool IsHalfStar { get; set; }
    }

    public class Ingredient
    {
        [JsonPropertyName("@id")]
        public string? Id { get; set; }

        [JsonPropertyName("ingredients")]
        public List<Ingredient>? Ingredients { get; set; }

        [JsonPropertyName("@type")]
        public string? Type { get; set; }

        [JsonPropertyName("ingredientText")]
        public string? IngredientText { get; set; }

        [JsonPropertyName("quantityText")]
        public string? QuantityText { get; set; }

        [JsonPropertyName("glossaryLink")]
        public string? GlossaryLink { get; set; }

        [JsonPropertyName("term")]
        public Term? Term { get; set; }

        [JsonPropertyName("note")]
        public string? Note { get; set; }
    }
    public class Term
    {
        [JsonPropertyName("@id")]
        public string? Id { get; set; }

        [JsonPropertyName("@type")]
        public string? Type { get; set; }

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        [JsonPropertyName("display")]
        public string? Display { get; set; }

        [JsonPropertyName("taxonomy")]
        public string? Taxonomy { get; set; }
    }

    public class Diet
    {
        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        [JsonPropertyName("display")]
        public string? Display { get; set; }

        [JsonPropertyName("taxonomy")]
        public string? Taxonomy { get; set; }
    }

    public class CookAndPrepTime
    {
        [JsonPropertyName("preparationMin")]
        public int PreparationMin { get; set; }

        [JsonPropertyName("preparationMax")]
        public int PreparationMax { get; set; }

        [JsonPropertyName("cookingMin")]
        public int CookingMin { get; set; }

        [JsonPropertyName("cookingMax")]
        public int CookingMax { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class Schema
    {

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("datePublished")]
        public string? DatePublished { get; set; } = null!;

        [JsonPropertyName("keywords")]
        public string? Keywords { get; set; }

        [JsonPropertyName("recipeCategory")]
        public string? RecipeCategory { get; set; }

        [JsonPropertyName("recipeCuisine")]
        public string? RecipeCuisine { get; set; }

        [JsonPropertyName("recipeIngredient")]
        public List<string>? RecipeIngredient { get; set; }

        [JsonPropertyName("recipeInstructions")]
        public List<RecipeInstruction>? RecipeInstructions { get; set; }

        [JsonPropertyName("recipeYield")]
        public string? RecipeYield { get; set; }
    }

    public class RecipeInstruction
    {
        [JsonPropertyName("@type")]
        public string? Type { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }

    public class NutritionalInfo
    {
        [JsonPropertyName("label")]
        public string? Label { get; set; }

        [JsonPropertyName("value")]
        public double Value { get; set; }

        [JsonPropertyName("prefix")]
        public string? Prefix { get; set; }

        [JsonPropertyName("suffix")]
        public string? Suffix { get; set; }


    }

    public class RecipeImage
    {
        [JsonPropertyName("@id")]
        public string? Id { get; set; }

        [JsonPropertyName("@type")]
        public string? Type { get; set; }

        [JsonPropertyName("alt")]
        public string? Alt { get; set; }

        [JsonPropertyName("aspectRatio")]
        public double AspectRatio { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("sourceName")]
        public object? SourceName { get; set; }
    }
}
