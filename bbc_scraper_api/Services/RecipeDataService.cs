using bbc_scraper_api.Contexts;
using bbc_scraper_api.MariaDBModels;
using bbc_scraper_api.Models;
using Microsoft.EntityFrameworkCore;
using Diet = bbc_scraper_api.MariaDBModels.Diet;
using Image = bbc_scraper_api.MariaDBModels.Image;
using Ingredient = bbc_scraper_api.MariaDBModels.Ingredient;
using IngredientGroup = bbc_scraper_api.MariaDBModels.IngredientGroup;
using NutritionalInfo = bbc_scraper_api.MariaDBModels.NutritionalInfo;
using Term = bbc_scraper_api.MariaDBModels.Term;

namespace bbc_scraper_api.Services;

public class RecipeDataService
{
    private readonly string _connectionString;

    public RecipeDataService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Category> FetchOrAddCategory(string query)
    {
        await using var dbContext = new RecipeDbContext(_connectionString);
        var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(c => c.Name == query);
        if (existingCategory != null)
            return existingCategory;

        var newCategory = new Category
        {
            Name = query
        };
        dbContext.Categories.Add(newCategory);
        await dbContext.SaveChangesAsync();
        return newCategory;
    }

    private async Task<Cuisine> FetchOrAddCuisine(string query)
    {
        using (var dbContext = new RecipeDbContext(_connectionString))
        {
            var existingCuisine = await dbContext.Cuisines.FirstOrDefaultAsync(c => c.Name == query);
            if (existingCuisine != null)
                return existingCuisine;

            var newCuisine = new Cuisine
            {
                Name = query
            };
            dbContext.Cuisines.Add(newCuisine);
            await dbContext.SaveChangesAsync();
            return newCuisine;
        }
    }

    private async Task<Keyword> FetchOrAddKeyword(string query)
    {
        using (var dbContext = new RecipeDbContext(_connectionString))
        {
            var existingKeyword = await dbContext.Keywords.FirstOrDefaultAsync(c => c.Name == query);
            if (existingKeyword != null)
                return existingKeyword;

            var newKeyword = new Keyword
            {
                Name = query
            };
            dbContext.Keywords.Add(newKeyword);
            await dbContext.SaveChangesAsync();
            return newKeyword;
        }
    }

    private async Task<Diet> FetchOrAddDiet(string query)
    {
        using (var dbContext = new RecipeDbContext(_connectionString))
        {
            var existingDiet = await dbContext.Diets.FirstOrDefaultAsync(c => c.Name == query);
            if (existingDiet != null)
                return existingDiet;

            var newDiet = new Diet
            {
                Name = query
            };
            dbContext.Diets.Add(newDiet);
            await dbContext.SaveChangesAsync();
            return newDiet;
        }
    }

    public async Task FetchOrAddRecipe(PageProps pageProps, List<ContentAPIResponse> contentApiResponses)
    {
        try
        {
            using (var dbContext = new RecipeDbContext(_connectionString))
            {
                var oldRecipe =
                    await dbContext.Recipes.FirstOrDefaultAsync(r => r.Id == Convert.ToInt32(pageProps.PostId));
                if (oldRecipe != null)
                    return;

                var categoryList = new List<Category>();
                if (pageProps?.Schema.RecipeCategory != null)
                    foreach (var category in pageProps?.Schema?.RecipeCategory?.Split(", "))
                        categoryList.Add(await FetchOrAddCategory(category));

                var cuisineList = new List<Cuisine>();
                if (pageProps?.Schema?.RecipeCuisine != null)
                    foreach (var cuisine in pageProps?.Schema?.RecipeCuisine?.Split(", "))
                        cuisineList.Add(await FetchOrAddCuisine(cuisine));

                var dietList = new List<Diet>();
                foreach (var cuisine in pageProps?.Diet?.Select(diet => diet.Display))
                    dietList.Add(await FetchOrAddDiet(cuisine));

                var ingredientGroupList = new List<IngredientGroup>();
                foreach (var group in pageProps.IngredientGroups)
                {
                    var ingredientList = new List<Ingredient>();
                    foreach (var i in group.Ingredients)
                    {
                        var ingredient = new Ingredient
                        {
                            Type = i.Type,
                            IngredientText = i.IngredientText,
                            Note = i.Note,
                            QuantityText = i.QuantityText,
                            Term = new Term
                            {
                                Display = i.Term?.Display,
                                Slug = i.Term?.Slug,
                                Taxonomy = i.Term?.Taxonomy,
                                Type = i.Term?.Type
                            }
                        };
                        ingredientList.Add(ingredient);
                    }

                    var ingredientGroup = new IngredientGroup
                    {
                        Heading = group.Heading,
                        Ingredients = ingredientList
                    };
                    ingredientGroupList.Add(ingredientGroup);
                }

                var keywordList = new List<Keyword>();
                foreach (var word in pageProps?.Schema?.Keywords.Split(", "))
                    keywordList.Add(await FetchOrAddKeyword(word));

                var newRecipe = new Recipe();

                newRecipe.Id = Convert.ToInt32(pageProps.PostId);
                newRecipe.Categories = categoryList;
                newRecipe.Cuisines = cuisineList;
                newRecipe.Description = pageProps?.Schema?.Description;
                newRecipe.Date = Convert.ToDateTime(pageProps?.Schema?.DatePublished);
                newRecipe.Diets = dietList;
                newRecipe.Image = new Image
                {
                    Alt = pageProps.Image.Alt,
                    Url = pageProps.Image.Url,
                    Height = pageProps.Image.Height,
                    Title = pageProps.Image.Title,
                    Width = pageProps.Image.Width,
                    AspectRatio = pageProps.Image.AspectRatio
                };
                newRecipe.Ingredients = ingredientGroupList;
                newRecipe.Instructions = pageProps?.Schema?.RecipeInstructions?.ConvertAll(i => new Instruction
                {
                    Type = i.Type,
                    Text = i.Text
                });
                newRecipe.Keywords = keywordList;
                newRecipe.Rating = new Rating
                {
                    Avg = pageProps.UserRatings.Avg,
                    IsHalfStar = pageProps.UserRatings.IsHalfStar,
                    Total = pageProps.UserRatings.Total
                };
                newRecipe.Name = pageProps?.Schema?.Name;
                newRecipe.NutritionalInfo = pageProps?.NutritionalInfo?.ConvertAll(i => new NutritionalInfo
                {
                    Label = i.Label,
                    Prefix = i.Prefix,
                    Suffix = i.Suffix,
                    Value = i.Value
                });
                newRecipe.SkillLevel = pageProps?.SkillLevel;
                newRecipe.Slug = pageProps?.Slug;
                newRecipe.Time = new Time
                {
                    PrepTime = pageProps.CookAndPrepTime.PreparationMax / 60,
                    CookTime = pageProps.CookAndPrepTime.CookingMax / 60,
                    TotalTime = pageProps.CookAndPrepTime.Total / 60
                };
                newRecipe.Yield = pageProps?.Schema?.RecipeYield;
                newRecipe.SimilarRecipes = contentApiResponses?.ConvertAll(content =>
                    new SimilarRecipe
                    {
                        Title = content.Title,
                        Url = content.Url,
                        Rating = new Rating
                        {
                            Avg = Convert.ToDouble(content.Rating.RatingValue),
                            IsHalfStar = content.Rating.IsHalfStar,
                            Total = content.Rating.RatingCount
                        }
                    }) ?? new List<SimilarRecipe>();

                dbContext.Recipes.Add(newRecipe);
                Console.WriteLine("Adding " + pageProps?.Schema?.Name);
                await dbContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    public async Task<List<int>> GetExistingRecipeIds(List<int> recipeIds)
    {
        await using var dbContext = new RecipeDbContext(_connectionString);
        return await dbContext.Recipes.Where(record => recipeIds.Contains(record.Id)).Select(r => r.Id).ToListAsync();
    }
}