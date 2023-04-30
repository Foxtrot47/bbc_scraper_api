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
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

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

app.MapGet("/search", (async (string query, RecipeDataService service) => {
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
}));

async Task FetchAndAddRecipe (Item item, RecipeDataService service)
{
    if (!await service.ItemExists(Convert.ToInt32(item.Id)))
    {
        var recipeData = await GetRecipeData(item);
        if (recipeData?.Props != null)
        {
            await GetContentApiResponse(Convert.ToInt32(item.Id), recipeData.Props.PageProps.Schema.Name);
            var data = new RecipeDataModel()
            {
                Id = Convert.ToInt32(item.Id),
                Name = recipeData.Props.PageProps.Schema.Name,
                Description = recipeData.Props.PageProps.Schema.Description,
                Slug = recipeData.Props.PageProps.Slug,
                Date = recipeData.Props.PageProps.Schema.DatePublished,
                Rating = new RecipeDataRating()
                {
                    Average = recipeData.Props.PageProps.UserRatings.Avg,
                    IsHalfStar = recipeData.Props.PageProps.UserRatings.IsHalfStar,
                    Total = recipeData.Props.PageProps.UserRatings.Total
                },
                Keywords = recipeData.Props.PageProps.Schema.Keywords.Split(", ").ToList(),
                NutritionalInfo = recipeData.Props.PageProps.NutritionalInfo.ConvertAll(
                    nutritionInfo => new RecipeDataNutritionalInfo()
                    {
                        Label= nutritionInfo.Label,
                        Prefix= nutritionInfo.Prefix,
                        Suffix= nutritionInfo.Suffix,
                        Value = nutritionInfo.Value
                    }),
                Category = recipeData.Props.PageProps.Schema.RecipeCategory.Split(", ").ToList(),
            };
            Console.WriteLine("Adding" + data.Name);
            await service.CreateAsync(data);
        }
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
        if (textContent != null)
            return JsonSerializer.Deserialize<BBCRecipeResponse>(textContent);
    }
    catch(Exception)
    {
        
    }
    return new BBCRecipeResponse();
}

async Task<List<ContentAPIResponse>?> GetContentApiResponse(int recipeId, string recipeName)
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

    return await client.GetJsonAsync<List<ContentAPIResponse>>("?contentRequest=" + urlEncodedText);
}

app.Run();

