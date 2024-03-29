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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using bbc_scraper_api.Contexts;
using Microsoft.EntityFrameworkCore;
using bbc_scraper_api.MariaDBModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RecipeDatabaseSettings>(
    builder.Configuration.GetSection("RecipeDatabase"));

builder.Services.AddDbContext<RecipeDbContext>(dbContextOptions => dbContextOptions
    .UseMySql(builder.Configuration.GetConnectionString("MariaDbConnectionString"), new MariaDbServerVersion((new Version(10, 6, 11))))
    .LogTo(Console.WriteLine, LogLevel.Information)
    .EnableDetailedErrors()
);

builder.Services.AddSingleton(provider =>
{
    var dbContext = provider.GetRequiredService<RecipeDbContext>();
    return new RecipeDataService(dbContext);
});

builder.Services.AddSingleton<RecipeDataServiceOld>();

builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

const string bbcBaseAPI = "https://www.bbcgoodfood.com/api/";
const string bbbcContentAPI = "https://related-content-production.headless.imdserve.com/";
var httpClient = new RestClient(new RestClientOptions(bbcBaseAPI));

app.MapGet("/search", async (string query, bool fetchSinglePage, RecipeDataService service) =>
{
    int fetchLimit = fetchSinglePage ? 1 : 100;
    List<Item> items;
    List<int> itemIds;
    List<int> existingItems;

    Result? responseResult = null;
    var firstFetch = true;
    do
    {
        if (firstFetch)
        {
            responseResult =
                await httpClient.GetJsonAsync<Result>(
                    bbcBaseAPI + "search-frontend/search?search=" + query + "&limit=" + fetchLimit);
            firstFetch = false;
        }
        else
        {
            responseResult = await httpClient.GetJsonAsync<Result>(responseResult?.SearchResults.NextUrl);
        }

        if (responseResult?.SearchResults == null || responseResult?.SearchResults.TotalItems < 1)
            break;

        items = new(responseResult?.SearchResults.Items);
        itemIds = items.ConvertAll(item => int.Parse(item.Id));
        existingItems = await service.GetExistingRecipeIds(itemIds);
        items = items.Where(item => !existingItems.Contains(int.Parse(item.Id))).ToList();

        var tasks = items?.Select(item => FetchAndAddRecipe(item, service));
        if (tasks != null) await Task.WhenAll(tasks);
    } while (responseResult?.SearchResults.NextUrl != null && fetchLimit > 1);
});

app.MapGet("/scrapecollection", async (string collectionSlug, RecipeDataServiceOld service) =>
{
    try
    {
        var pageNum = 1;
        BbcCollectionApiResponseRoot response;
        do
        {
            var query = bbcBaseAPI + "lists/posts/list/" + collectionSlug + "/items?page=" + pageNum;
            response = await httpClient.GetJsonAsync<BbcCollectionApiResponseRoot>(query);
            if (response?.Items == null)
                break;
            foreach (var item in response.Items)
            {
                var collectionItem = new CollectionModel();
                TextInfo info = new CultureInfo("en-US", false).TextInfo;
                collectionItem.CollectionName = info.ToTitleCase(collectionSlug.Replace("-", " "));
                collectionItem.CollectionSlug = collectionSlug;
                collectionItem.RecipeNameName = item.Name;
                collectionItem.Image = item.Image;
                collectionItem.Url = item.Url;
                collectionItem.DateAdded = DateTime.Now;
                collectionItem.Rating = new RecipeDataRating()
                {
                    Total = item.Rating.RatingCount,
                    IsHalfStar = item.Rating.IsHalfStar,
                    Average = (float)item.Rating.RatingValue
                };
                await service.CreateCollectionItemAsync(collectionItem);
            }
            pageNum++;
        } while (!string.IsNullOrEmpty(response.NextUrl));
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }
});

async Task FetchAndAddRecipe(Item item, RecipeDataService service)
{
    try
    {
        var recipeData = await GetRecipeData(item);
        if (recipeData?.Props != null)
        {
            var contentData = recipeData.Props.PageProps?.Schema?.Name != null ? await GetContentApiResponse(Convert.ToInt32(item.Id), recipeData.Props.PageProps.Schema.Name) : null;
            //var newrecipe = new Recipe
            //{
            //    Id = Convert.ToInt32(item.Id),
            //    Name = recipeData.Props.PageProps?.Schema?.Name,
            //    Description = recipeData.Props.PageProps?.Schema?.Description,
            //    Slug = recipeData.Props.PageProps?.Slug,
            //    Date = DateTime.Parse(recipeData.Props.PageProps?.Schema?.DatePublished),
            //    Rating = recipeData.Props.PageProps?.UserRatings != null
            //        ? new Rating
            //        {
            //            Avg = recipeData.Props.PageProps.UserRatings.Avg,
            //            IsHalfStar = recipeData.Props.PageProps.UserRatings.IsHalfStar,
            //            Total = recipeData.Props.PageProps.UserRatings.Total
            //        }
            //        : null,
            //    Keywords = recipeData.Props.PageProps?.Schema?.Keywords?.Split(", ").ToList(),
            //    NutritionalInfo = recipeData.Props.PageProps?.NutritionalInfo?.ConvertAll(nutritionInfo =>
            //        new bbc_scraper_api.MariaDBModels.NutritionalInfo
            //        {
            //            Label = nutritionInfo.Label,
            //            Prefix = nutritionInfo.Prefix,
            //            Suffix = nutritionInfo.Suffix,
            //            Value = nutritionInfo.Value
            //        }),
            //    Category = recipeData.Props.PageProps?.Schema?.RecipeCategory?.Split(", ").ToList(),
            //    Diet = recipeData.Props.PageProps?.Diet?.ConvertAll(dietInfo =>
            //        new RecipeDataDiet
            //        {
            //            Slug = dietInfo.Slug,
            //            Display = dietInfo.Display,
            //            Taxonomy = dietInfo.Taxonomy
            //        }) ?? new List<RecipeDataDiet>(),
            //    Cusine = recipeData.Props.PageProps?.Schema?.RecipeCuisine != null
            //        ? new List<string> { recipeData.Props.PageProps.Schema.RecipeCuisine }
            //        : null,
            //    Ingredients = recipeData.Props.PageProps?.Ingredients?.ConvertAll(ingredient =>
            //        new RecipeDataIngredientsModel
            //        {
            //            Ingredients = ingredient.Ingredients?.ConvertAll(_ingredient =>
            //                new RecipeDataIngredientModel
            //                {
            //                    Note = _ingredient.Note,
            //                    Term = _ingredient.Term != null
            //                        ? new RecipeDataIngredientTerm
            //                        {
            //                            Type = _ingredient.Term.Type,
            //                            Display = _ingredient.Term.Display,
            //                            Taxonomy = _ingredient.Term.Taxonomy,
            //                            Slug = _ingredient.Term.Slug,
            //                            Id = _ingredient.Term.Id
            //                        }
            //                        : null,
            //                    Type = _ingredient.Type,
            //                    IngredientText = _ingredient.IngredientText,
            //                    QuantityText = _ingredient.QuantityText
            //                })
            //        }),
            //    Instructions = recipeData.Props.PageProps?.Schema?.RecipeInstructions?.ConvertAll(instruction =>
            //        new RecipeDataInstructions
            //        {
            //            Type = instruction.Type,
            //            Text = instruction.Text
            //        }),
            //    Yield = recipeData.Props.PageProps?.Schema?.RecipeYield,
            //    Image = recipeData.Props.PageProps?.Image != null
            //        ? new RecipeDataImage
            //        {
            //            Alt = recipeData.Props.PageProps.Image.Alt,
            //            Url = recipeData.Props.PageProps.Image.Url,
            //            Height = recipeData.Props.PageProps.Image.Height,
            //            Title = recipeData.Props.PageProps.Image.Title,
            //            Width = recipeData.Props.PageProps.Image.Width,
            //            AspectRatio = recipeData.Props.PageProps.Image.AspectRatio
            //        }
            //        : null,
            //    SkillLevel = recipeData.Props.PageProps?.SkillLevel,
            //    Time = recipeData.Props.PageProps?.CookAndPrepTime != null
            //        ? new RecipeDataTime
            //        {
            //            PrepTime = recipeData.Props.PageProps.CookAndPrepTime.PreparationMax / 60,
            //            CookTime = recipeData.Props.PageProps.CookAndPrepTime.CookingMax / 60,
            //            TotalTime = recipeData.Props.PageProps.CookAndPrepTime.Total / 60
            //        }
            //        : null,
            //    SimiliarRecipes = contentData?.ConvertAll(content =>
            //        new SimiliarRecipeData
            //        {
            //            Image = new SimiliarRecipeDataImage
            //            {
            //                Alt = content.Image.Alt,
            //                Height = content.Image.Height,
            //                Title = content.Image.Title,
            //                Width = content.Image.Width,
            //                AspectRatio = content.Image.AspectRatio,
            //                Url = content.Image.Url
            //            },
            //            Title = content.Title,
            //            Url = content.Url,
            //            Rating = content.Rating
            //        }) ?? new List<SimiliarRecipeData>()
            //};
            //Console.WriteLine("Adding" + data.Name);
            //await service.CreateAsync(data);
        }
    }
    catch (Exception ex)
    {
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

