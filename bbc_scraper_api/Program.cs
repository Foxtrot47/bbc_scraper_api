using AngleSharp;
using AngleSharp.Dom;
using bbc_scraper_api.Models;
using bbc_scraper_api.Services;
using Microsoft.AspNetCore.Http.Json;
using RestSharp;
using RestSharp.Serializers.Xml;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using bbc_scraper_api.Utils;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RecipeDatabaseSettings>(
    builder.Configuration.GetSection("RecipeDatabase"));

builder.Services.AddSingleton<RecipeDataService>();

builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

const string bbcBaseAPI = "https://www.bbcgoodfood.com/api/search-frontend/";
const string bbbcContentAPI = "https://related-content-production.headless.imdserve.com/";
var httpClient = new RestClient(new RestClientOptions(bbcBaseAPI));

app.MapGet("/search", async (string query, RecipeDataService service) =>
{
    var fetchResponse = await httpClient.GetJsonAsync<Result>(bbcBaseAPI + "search?search=" + query + "&limit=100");
    if (fetchResponse?.SearchResults.TotalItems > 0)
    {
        List<Item> items = new(fetchResponse.SearchResults.Items);

        while (fetchResponse.SearchResults.NextUrl != null)
        {
            fetchResponse = await httpClient.GetJsonAsync<Result>(fetchResponse.SearchResults.NextUrl);
            items.AddRange(items);
        }
        const int threadMaxSize = 12;
        int batchCount = (int)Math.Ceiling((double)items.Count / threadMaxSize);
        for (int i = 0; i < batchCount; i++)
        {
            var batchedItems = items.Skip(i * threadMaxSize).Take(threadMaxSize);
            var tasks = batchedItems.Select(item => FetchAndAddRecipe(item, service));
            await Task.WhenAll(tasks);
        }
    }
});

async Task FetchAndAddRecipe(Item item, RecipeDataService service)
{
    try {
        if (!await service.ItemExists(Convert.ToInt32(item.Id)))
        {
            var recipeData = await GetRecipeData(item);
            if (recipeData?.Props != null)
            {
                var contentData = recipeData.Props.PageProps?.Schema?.Name != null ? await GetContentApiResponse(Convert.ToInt32(item.Id), recipeData.Props.PageProps.Schema.Name) : null;
                var data = new RecipeDataModel()
                {
                    Id = Convert.ToInt32(item.Id),
                    Name = recipeData.Props.PageProps?.Schema?.Name,
                    Description = recipeData.Props.PageProps?.Schema?.Description,
                    Slug = recipeData.Props.PageProps?.Slug,
                    Date = recipeData.Props.PageProps?.Schema?.DatePublished,
                    Rating = recipeData.Props.PageProps?.UserRatings != null ? new RecipeDataRating()
                    {
                        Average = recipeData.Props.PageProps.UserRatings.Avg,
                        IsHalfStar = recipeData.Props.PageProps.UserRatings.IsHalfStar,
                        Total = recipeData.Props.PageProps.UserRatings.Total
                    } : null,
                    Keywords = recipeData.Props.PageProps?.Schema?.Keywords?.Split(", ").ToList(),
                    NutritionalInfo = recipeData.Props.PageProps?.NutritionalInfo?.ConvertAll(
                        nutritionInfo => new RecipeDataNutritionalInfo()
                        {
                            Label = nutritionInfo.Label,
                            Prefix = nutritionInfo.Prefix,
                            Suffix = nutritionInfo.Suffix,
                            Value = nutritionInfo.Value
                        }),
                    Category = recipeData.Props.PageProps?.Schema?.RecipeCategory?.Split(", ").ToList(),
                    Diet = recipeData.Props.PageProps?.Diet != null ? recipeData.Props.PageProps.Diet.ConvertAll(dietInfo => new RecipeDataDiet()
                    {
                        Slug = dietInfo.Slug,
                        Display = dietInfo.Display,
                        Taxonomy = dietInfo.Taxonomy
                    }) : new List<RecipeDataDiet>(),
                    Cusine = recipeData.Props.PageProps?.Schema?.RecipeCuisine != null ? new List<string>() { recipeData.Props.PageProps.Schema.RecipeCuisine } : null,
                    Ingredients = recipeData.Props.PageProps?.Ingredients?.ConvertAll(ingredient =>
                        new RecipeDataIngredientsModel()
                        {
                            Ingredients = ingredient.Ingredients?.ConvertAll(
                                _ingredient => new RecipeDataIngredientModel()
                                {
                                    Note = _ingredient.Note,
                                    Term = _ingredient.Term != null ? new RecipeDataIngredientTerm()
                                    {
                                        Type = _ingredient.Term.Type,
                                        Display = _ingredient.Term.Display,
                                        Taxonomy = _ingredient.Term.Taxonomy,
                                        Slug = _ingredient.Term.Slug,
                                        Id = _ingredient.Term.Id
                                    } : null,
                                    Type = _ingredient.Type,
                                    IngredientText = _ingredient.IngredientText,
                                    QuantityText = _ingredient.QuantityText
                                })
                        }),
                    Instructions = recipeData.Props.PageProps?.Schema?.RecipeInstructions?.ConvertAll(instruction =>
                        new RecipeDataInstructions()
                        {
                            Type = instruction.Type,
                            Text = instruction.Text
                        }),
                    Yield = recipeData.Props.PageProps?.Schema?.RecipeYield,
                    Image = recipeData.Props.PageProps?.Image != null ? new RecipeDataImage()
                    {
                        Alt = recipeData.Props.PageProps.Image.Alt,
                        Url = recipeData.Props.PageProps.Image.Url,
                        Height = recipeData.Props.PageProps.Image.Height,
                        Title = recipeData.Props.PageProps.Image.Title,
                        Width = recipeData.Props.PageProps.Image.Width,
                        AspectRatio = recipeData.Props.PageProps.Image.AspectRatio
                    } : null,
                    SkillLevel = recipeData.Props.PageProps?.SkillLevel,
                    Time = recipeData.Props.PageProps?.CookAndPrepTime != null ? new RecipeDataTime()
                    {
                        PrepTime = recipeData.Props.PageProps.CookAndPrepTime.PreparationMax / 60,
                        CookTime = recipeData.Props.PageProps.CookAndPrepTime.CookingMax / 60,
                        TotalTime = recipeData.Props.PageProps.CookAndPrepTime.Total / 60,
                    } : null,
                    SimiliarRecipes = contentData != null ? contentData?.ConvertAll(content =>
                        new SimiliarRecipeData()
                        {
                            Image = new SimiliarRecipeDataImage()
                            {
                                Alt = content.Image.Alt,
                                Height = content.Image.Height,
                                Title = content.Image.Title,
                                Width = content.Image.Width,
                                AspectRatio = content.Image.AspectRatio,
                                Url = content.Image.Url
                            },
                            Title = content.Title,
                            Url = content.Url
                        }) : new List<SimiliarRecipeData>()
                };
                Console.WriteLine("Adding" + data.Name);
                await service.CreateAsync(data);
            }
        }
    }
    catch (Exception ex) {
        Console.WriteLine(ex);
    }
}

async Task<BBCRecipeResponse> GetRecipeData(Item item)
{
    try
    {
        var config = Configuration.Default.WithDefaultLoader();
        var address = "https://www.bbcgoodfood.com" + item.Url;
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync(address);
        var textContent = document.QuerySelector("#__NEXT_DATA__")?.TextContent;

        var options = new JsonSerializerOptions();
        options.Converters.Add(new CustomJsonConverter());
        if (textContent != null)
            return JsonSerializer.Deserialize<BBCRecipeResponse>(textContent, options);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
    return new BBCRecipeResponse();
}

async Task<List<ContentAPIResponse>?> GetContentApiResponse(int recipeId, string recipeName)
{
    var result = new List<ContentAPIResponse>();
    try
    {
        var client = new RestClient(bbbcContentAPI);
        var apiRequestBody = new ContentAPIRequest
        {
            siteKey = "bbcgoodfood",
            searchTerm = recipeName,
            postId = Convert.ToInt32(recipeId),
            widgetLimit = 8,
            type = new List<string>
            {
                "sxs-recipe"
            },
            showCardLabels = false,
            v5enabled = false
        };

        string urlEncodedText = HttpUtility.UrlEncode(JsonSerializer.Serialize(apiRequestBody));

        var req = new RestRequest("?contentRequest=" + urlEncodedText);
        var response = await client.GetAsync(req);

        var options = new JsonSerializerOptions();
        options.Converters.Add(new CustomJsonConverter());
        if (response.Content != null)
            result = JsonSerializer.Deserialize<List<ContentAPIResponse>>(response.Content, options);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }

    return result;
}

app.Run();

