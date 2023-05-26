using Microsoft.EntityFrameworkCore;
using bbc_scraper_api.MariaDBModels;
using bbc_scraper_api.Models;
using Diet = bbc_scraper_api.MariaDBModels.Diet;
using Image = bbc_scraper_api.MariaDBModels.Image;
using Ingredient = bbc_scraper_api.MariaDBModels.Ingredient;
using IngredientGroup = bbc_scraper_api.MariaDBModels.IngredientGroup;
using NutritionalInfo = bbc_scraper_api.MariaDBModels.NutritionalInfo;
using Term = bbc_scraper_api.MariaDBModels.Term;

namespace bbc_scraper_api.Contexts;

public class RecipeDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Cuisine> Cuisines { get; set; }
    public DbSet<Diet> Diets { get; set; }
    public DbSet<Keyword> Keywords { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<IngredientGroup> IngredientGroups { get; set; }
    public DbSet<Instruction> Instructions { get; set; }
    public DbSet<NutritionalInfo> NutritionalInfos { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<SimilarRecipe> SimilarRecipes { get; set; }
    public DbSet<Term> Terms { get; set; }
    public DbSet<Time> Times { get; set; }

    // public RecipeDbContext(DbContextOptions<RecipeDbContext> options) : base(options)
    // {
    // }
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     => optionsBuilder.UseNpgsql("Server=localhost;User ID=postgres;Password=Geforce35;Database=recipedb;Include Error Detail=true;");

    
    public RecipeDbContext(string connectionString) : base(GetOptions(connectionString))
    {
    }

    private static DbContextOptions GetOptions(string connectionString)
    {
        return new DbContextOptionsBuilder().UseNpgsql(connectionString).Options;
    }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recipe>()
            .HasMany(r => r.Categories)
            .WithMany(k => k.Recipes)
            .UsingEntity(j => j.ToTable("RecipeCategories"));

        modelBuilder.Entity<Recipe>()
            .HasMany(r => r.Cuisines)
            .WithMany(k => k.Recipes)
            .UsingEntity(j => j.ToTable("RecipeCuisines"));

        modelBuilder.Entity<Recipe>()
            .HasMany(r => r.Keywords)
            .WithMany(k => k.Recipes)
            .UsingEntity(j => j.ToTable("RecipeKeywords"));

        modelBuilder.Entity<Recipe>()
            .HasMany(r => r.Diets)
            .WithMany(k => k.Recipes)
            .UsingEntity(j => j.ToTable("RecipeDiets"));
    }
}