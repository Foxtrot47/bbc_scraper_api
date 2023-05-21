using Microsoft.EntityFrameworkCore;
using bbc_scraper_api.MariaDBModels;

namespace bbc_scraper_api.Contexts;

public class RecipeDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Keyword> Keywords { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<IngredientGroup> IngredientGroups { get; set; }
    public DbSet<Instruction> Instructions { get; set; }
    public DbSet<NutritionalInfo> NutritionalInfos { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<RecipeCategory> RecipeCategories { get; set; }
    public DbSet<RecipeCuisine> RecipeCuisines { get; set; }
    public DbSet<RecipeDiet> RecipeDiets { get; set; }
    public DbSet<RecipeKeyword> RecipeKeywords { get; set; }
    public DbSet<SimilarRecipe> SimilarRecipes { get; set; }
    public DbSet<SimilarRecipeRating> SimilarRecipeRatings { get; set; }
    public Term Terms { get; set; }
    public Time Times { get; set; }
    

    public RecipeDbContext(DbContextOptions<RecipeDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecipeKeyword>()
            .HasKey(rk => new { rk.RecipeId, rk.KeywordId });

        modelBuilder.Entity<RecipeCategory>()
            .HasKey(rc => new { rc.RecipeId, rc.CategoryId });
    }
}