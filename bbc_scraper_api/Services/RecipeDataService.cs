using bbc_scraper_api.MariaDBModels;
using bbc_scraper_api.Models;
using Diet = bbc_scraper_api.MariaDBModels.Diet;
using Image = bbc_scraper_api.MariaDBModels.Image;
using Ingredient = bbc_scraper_api.MariaDBModels.Ingredient;
using IngredientGroup = bbc_scraper_api.MariaDBModels.IngredientGroup;
using NutritionalInfo = bbc_scraper_api.MariaDBModels.NutritionalInfo;
using Term = bbc_scraper_api.MariaDBModels.Term;
using Dapper;
using Npgsql;

namespace bbc_scraper_api.Services;

public class RecipeDataService
{
    private readonly string _connectionString;

    public RecipeDataService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<int> FetchOrAddCategory(string query)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var sql = "SELECT * FROM Categories  WHERE name = @Name LIMIT 1";
        var results = await connection.QueryAsync<Category>(sql, new { Name = query });

        if (results?.ToList().Count > 0)
            return results.First().Id;

        sql = "INSERT INTO Categories (name) VALUES (@Value) RETURNING id";
        return await connection.ExecuteScalarAsync<int>(sql, new { Value = query });
    }

    private async Task<int> FetchOrAddCuisine(string query)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var sql = "SELECT * FROM Cuisines  WHERE name = @Name LIMIT 1";
        var results = await connection.QueryAsync<Cuisine>(sql, new { Name = query });

        if (results?.ToList().Count > 0)
            return results.First().Id;

        sql = "INSERT INTO Cuisines (name) VALUES (@Value) RETURNING id";
        return await connection.ExecuteScalarAsync<int>(sql, new { Value = query });
    }

    private async Task<int> FetchOrAddDiet(string query)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var sql = "SELECT * FROM Diets WHERE name = @Name LIMIT 1";
        var results = await connection.QueryAsync<Diet>(sql, new { Name = query });

        if (results?.ToList().Count > 0)
            return results.First().Id;

        sql = "INSERT INTO Diets (name) VALUES (@Value) RETURNING id";
        return await connection.ExecuteScalarAsync<int>(sql, new { Value = query });
    }
    private async Task<int> FetchOrAddKeyword(string query)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var sql = "SELECT * FROM Keywords WHERE name = @Name LIMIT 1";
        var results = await connection.QueryAsync<Keyword>(sql, new { Name = query });

        if (results?.ToList().Count > 0)
            return results.First().Id;

        sql = "INSERT INTO Keywords (name) VALUES (@Value) RETURNING id";
        return await connection.ExecuteScalarAsync<int>(sql, new { Value = query });
    }

    private async Task<int> FetchOrAddTerm(string display, string slug, string taxonomy, string type)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var sql = "SELECT * FROM terms WHERE display = @Display LIMIT 1";
        var results = await connection.QueryAsync<Term>(sql, new { Display = display });

        if (results?.ToList().Count > 0)
            return results.First().Id;

        sql = "INSERT INTO terms (display, slug, taxonomy,type) VALUES (@Display, @Slug, @Taxonomy, @Type) RETURNING id";
        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            display,
            slug,
            taxonomy,
            type
        });
    }

    public async Task FetchOrAddRecipe(PageProps pageProps, List<ContentAPIResponse> contentApiResponses)
    {
        try
        {
            var date = DateTime.Now;
            Console.WriteLine($"Fetch fn for ${pageProps.Schema.Name} starting at ${date}");
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            var sql = "INSERT INTO images (url,title,alt,width,height,aspectratio) VALUES (@Url, @Title, @Alt, @Width, @Height, @AspectRatio) RETURNING id";
            var ImageId = await connection.ExecuteScalarAsync<int>(sql, new
            {
                pageProps?.Image?.Alt,
                pageProps?.Image?.Url,
                pageProps?.Image?.Height,
                pageProps?.Image?.Title,
                pageProps?.Image?.Width,
                pageProps?.Image?.AspectRatio
            });

            sql = "INSERT INTO ratings (avg,ishalfstar,total) VALUES (@Avg, @IsHalfStar, @Total) RETURNING id";
            var RatingId = await connection.ExecuteScalarAsync<int>(sql, new
            {
                pageProps?.UserRatings?.Avg,
                pageProps?.UserRatings?.IsHalfStar,
                pageProps?.UserRatings?.Total
            });

            sql = "INSERT INTO times (cooktime,preptime,totaltime) VALUES (@PrepTime, @CookTime, @TotalTime) RETURNING id";
            var TimeId = await connection.ExecuteScalarAsync<int>(sql, new
            {
                PrepTime = pageProps.CookAndPrepTime.PreparationMax / 60,
                CookTime = pageProps.CookAndPrepTime.CookingMax / 60,
                TotalTime = pageProps.CookAndPrepTime.Total / 60
            });

            sql = "INSERT INTO recipes (id,date,description,imageid,name,ratingid,slug,skilllevel,timeid,yield) VALUES (@Id,@Date,@Description,@ImageId,@Name,@RatingId,@Slug,@SkillLevel,@TimeId,@Yield) RETURNING id";
            var RecipeId = await connection.ExecuteScalarAsync<int>(sql, new
            {
                Id = Convert.ToInt32(pageProps.PostId),
                Date = Convert.ToDateTime(pageProps?.Schema?.DatePublished),
                pageProps?.Schema?.Description,
                ImageId,
                pageProps?.Schema?.Name,
                RatingId,
                pageProps?.Slug,
                pageProps?.SkillLevel,
                TimeId,
                Yield = pageProps?.Schema?.RecipeYield
            });

            var categoryList = new List<RecipeCategory>();
            if (pageProps?.Schema?.RecipeCategory != null)
            {
                foreach (var category in pageProps?.Schema?.RecipeCategory?.Split(", "))
                {
                    categoryList.Add(new RecipeCategory
                    {
                        RecipeId = RecipeId,
                        CategoryId = await FetchOrAddCategory(category)
                    });
                }

                sql = "INSERT INTO recipecategories (recipeid,categoryid) VALUES (@RecipeId, @CategoryId)";
                await connection.ExecuteAsync(sql, categoryList);
            }

            var cuisineList = new List<RecipeCuisine>();
            if (pageProps?.Schema?.RecipeCuisine != null)
            {
                foreach (var cuisine in pageProps?.Schema?.RecipeCuisine?.Split(", "))
                {
                    cuisineList.Add(new RecipeCuisine
                    {
                        RecipeId = RecipeId,
                        CuisineId = await FetchOrAddCuisine(cuisine)
                    });
                }
                sql = "INSERT INTO recipecuisines (recipeid,cuisineid) VALUES (@RecipeId, @CuisineId)";
                await connection.ExecuteAsync(sql, cuisineList);
            }

            var dietList = new List<RecipeDiet>();
            if (pageProps?.Diet != null)
            {
                foreach (var diet in pageProps?.Diet?.Select(diet => diet.Display))
                {
                    dietList.Add(new RecipeDiet
                    {
                        RecipeId = RecipeId,
                        DietId = await FetchOrAddDiet(diet)
                    });
                }
                sql = "INSERT INTO recipediets (recipeid,dietid) VALUES (@RecipeId, @DietId)";
                await connection.ExecuteAsync(sql, dietList);
            }

            var keywordList = new List<RecipeKeyword>();
            if (pageProps?.Schema?.Keywords != null)
            {
                foreach (var keyword in pageProps?.Schema?.Keywords?.Split(", "))
                {
                    keywordList.Add(new RecipeKeyword
                    {
                        RecipeId = RecipeId,
                        KeywordId = await FetchOrAddKeyword(keyword)
                    });
                }
                sql = "INSERT INTO recipekeywords (recipeid,keywordid) VALUES (@RecipeId, @KeywordId)";
                await connection.ExecuteAsync(sql, keywordList);
            }


            foreach (var group in pageProps.IngredientGroups)
            {
                sql = "INSERT INTO ingredientgroups (heading,recipeid) VALUES (@Heading,@RecipeId) RETURNING id";
                var GroupId = await connection.ExecuteScalarAsync<int>(sql, new { group.Heading, RecipeId = Convert.ToInt32(pageProps.PostId) });

                var ingredientList = new List<Ingredient>();
                foreach (var i in group.Ingredients)
                {
                    var TermId = await FetchOrAddTerm(i.Term?.Display, i.Term?.Slug, i.Term?.Taxonomy, i.Term?.Type);

                    sql = "INSERT INTO ingredients (type,ingredienttext,note,quantitytext,termid,ingredientgroupid) VALUES (@Type, @IngredientText, @Note, @QuantityText, @TermId, @GroupId) RETURNING id";
                    var ingredientId = await connection.QueryAsync<int>(sql, new
                    {
                        type = i.Type,
                        i.IngredientText,
                        i.Note,
                        i.QuantityText,
                        TermId,
                        GroupId
                    });
                }
            }

            var instructionList = pageProps?.Schema?.RecipeInstructions?.ConvertAll(i => new Instruction
            {
                RecipeId = RecipeId,
                Type = i.Type,
                Text = i.Text
            });

            sql = "INSERT INTO instructions (type,text,recipeid) VALUES (@Type,@Text,@RecipeId)";
            await connection.ExecuteAsync(sql, instructionList);

            var nutritionalInfoList = pageProps?.NutritionalInfo?.ConvertAll(i => new NutritionalInfo
            {
                Label = i.Label,
                Prefix = i.Prefix,
                Suffix = i.Suffix,
                Value = i.Value,
                RecipeId = RecipeId
            });

            sql = "INSERT INTO nutritionalinfos (label,prefix,suffix,value,recipeid) VALUES (@Label,@Prefix,@Suffix,@Value,@RecipeId)";
            await connection.ExecuteAsync(sql, nutritionalInfoList);

            // Insert similar recipes
            await InsertSimilarRecipesAsync(connection, RecipeId, contentApiResponses);

            transaction.Commit();
            var nextDate = DateTime.Now;
            var diff = (date - nextDate).TotalSeconds;
            Console.WriteLine($"Fetch fn for {pageProps.Schema.Name} ended at {nextDate} total seconds taken {diff}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    public async Task<List<int>> GetExistingRecipeIds(List<int> recipeIds)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var sql = "SELECT * FROM recipes  WHERE id = ANY(@Ids)";
        var results = await connection.QueryAsync<Recipe>(sql, new { Ids = recipeIds });
        return results.Select(r => r.Id).ToList();
    }
    private static async Task InsertSimilarRecipesAsync(NpgsqlConnection connection, int recipeId, List<ContentAPIResponse> contentApiResponses)
    {
        var similarRecipes = contentApiResponses
            .ConvertAll(c => new SimilarRecipe
            {
                RecipeId = recipeId,
                SimiliarRecipeId = Convert.ToInt32(c.Id)
            });

        if (similarRecipes.Any())
        {
            const string sql = "INSERT INTO similarrecipes (recipeid, similiarrecipeid) " +
                      "VALUES (@RecipeId, @SimiliarRecipeId)";
            await connection.ExecuteAsync(sql, similarRecipes);
        }
    }
}