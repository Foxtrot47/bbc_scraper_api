namespace bbc_scraper_api.MariaDBModels;

public class Category
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public ICollection<RecipeCategory>? RecipeCategories { get; set; }
}

public class Cuisine
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public ICollection<RecipeCuisine>? RecipeCuisines { get; set; }
}

public class Diet
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public ICollection<RecipeDiet>? RecipeDiets { get; set; }
}

public class Keyword
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public ICollection<RecipeKeyword>? RecipeKeywords { get; set; }
}

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
    public List<Recipe>? Recipes { get; set; }
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
    public int RecipeId { get; set; }
}

public class NutritionalInfo
{
    public int Id { get; set; }
    public string? Label { get; set; }
    public string? Prefix { get; set; }
    public string? Suffix { get; set; }
    public double? Value { get; set; }
    public int RecipeId { get; set; }
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
    public ICollection<RecipeCategory>? RecipeCategories { get; set; }
    public ICollection<RecipeCuisine>? RecipeCuisines { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public ICollection<RecipeDiet>? RecipeDiets { get; set; }
    public Image? Image { get; set; }
    public ICollection<IngredientGroup>? Ingredients { get; set; }
    public ICollection<Instruction>? Instructions { get; set; }
    public ICollection<RecipeKeyword>? RecipeKeywords { get; set; }
    public string? Name { get; set; }
    public ICollection<NutritionalInfo>? NutritionalInfo { get; set; }
    public Rating? Rating { get; set; }
    public string? Slug { get; set; }
    public ICollection<SimilarRecipe>? SimilarRecipes { get; set; }
    public string? SkillLevel { get; set; }
    public Time? Time { get; set; }
    public string? Yield { get; set; }
}

public class RecipeCategory
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public Recipe? Recipe { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}

public class RecipeCuisine
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public Recipe? Recipe { get; set; }
    public int CuisineId { get; set; }
    public Cuisine? Cuisine { get; set; }
}
public class RecipeDiet
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public Recipe? Recipe { get; set; }
    public int DietId { get; set; }
    public Diet? Diet { get; set; }
}
public class RecipeKeyword
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public Recipe? Recipe { get; set; }
    public int KeywordId { get; set; }
    public Keyword? Keyword { get; set; }
}
public class SimilarRecipe
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public int SimiliarRecipeId { get; set; }
}

public class Term
{
    public int Id { get; set; }
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