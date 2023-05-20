namespace bbc_scraper_api.MariaDBModels;

public class Image
{
    public int Id { get; set; }
    public string? Url { get; set; }
    public string? Title { get; set; }
    public string? Alt { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public double AspectRatio { get; set; }
}

public class Ingredient
{
    public int Id { get; set; }
    public string? Type { get; set; }
    public string? IngredientText { get; set; }
    public string? Note { get; set; }
    public string? QuantityText { get; set; }
    public Term? Term { get; set; }
}

public class IngredientGroup
{
    public int Id { get; set; }
    public string? Heading { get; set; }
    public ICollection<Ingredient>? Ingredients { get; set; }
}

public class Instruction
{
    public int Id { get; set; }
    public string? Type { get; set; }
    public string? Text { get; set; }
}

public class NutritionalInfo
{
    public int Id { get; set; }
    public string? Label { get; set; }
    public string? Prefix { get; set; }
    public string? Suffix { get; set; }
    public double? Value { get; set; }
}

public class Rating
{
    public int Id { get; set; }
    public double Avg { get; set; }
    public bool IsHalfStar { get; set; }
    public int Total { get; set; }
}
public class Recipe
{
    public int Id { get; set; }
    public ICollection<RecipeCategory>? Categories { get; set; }
    public ICollection<RecipeCuisine>? Cuisine { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public ICollection<RecipeDiet>? Diet { get; set; }
    public Image? Image { get; set; }
    public ICollection<IngredientGroup>? Ingredients { get; set; }
    public ICollection<Instruction>? Instructions { get; set; }
    public ICollection<RecipeKeyword>? Keywords { get; set; }
    public string? Name { get; set; }
    public ICollection<NutritionalInfo>? NutritionalInfo { get; set; }
    public Rating? Rating { get; set; }
    public string Slug { get; set; }
    public ICollection<SimilarRecipe>? SimilarRecipes { get; set; }
    public string? SkillLevel { get; set; }
    public Time? Time { get; set; }
    public string? Yield { get; set; }
}

public class RecipeCategory
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class RecipeCuisine
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class RecipeDiet
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public string? Name { get; set; }
}

public class RecipeKeyword
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public string? Name { get; set; }
}
public class SimilarRecipe
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Url { get; set; }
    public Image? Image { get; set; }
    public SimilarRecipeRating? Rating { get; set; }
}

public class SimilarRecipeRating
{
    public int Id { get; set; }
    public string? RatingValue { get; set; }
    public string? RatingCount { get; set; }
    public string? RatingTypeLabel { get; set; }
    public bool HasRatingCount { get; set; }
    public bool IsHalfStar { get; set; }
}

public class Term
{
    public string? Id { get; set; }
    public string? Type { get; set; }
    public string? Slug { get; set; }
    public string? Display { get; set; }
    public string? Taxonomy { get; set; }
}

public class Time
{
    public int Id { get; set; }
    public int CookTime { get; set; }
    public int PrepTime { get; set; }
    public int TotalTime { get; set; }
}
