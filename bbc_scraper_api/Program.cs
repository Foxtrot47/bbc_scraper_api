using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using AngleSharp;
using bbc_scraper_api.MariaDBModels;
using bbc_scraper_api.Models;
using bbc_scraper_api.Services;
using bbc_scraper_api.Utils;
using Microsoft.AspNetCore.Http.Json;
using RestSharp;
using Image = bbc_scraper_api.MariaDBModels.Image;
using NutritionalInfo = bbc_scraper_api.MariaDBModels.NutritionalInfo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<RecipeDataService>(provider => 
    new RecipeDataService(builder.Configuration.GetConnectionString("PostgreSQLConnectionString")));

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
    var fetchLimit = fetchSinglePage ? 1 : 100;
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

        items = new List<Item>(responseResult?.SearchResults.Items);
        itemIds = items.ConvertAll(item => int.Parse(item.Id));
        existingItems = await service.GetExistingRecipeIds(itemIds);
        items = items.Where(item => !existingItems.Contains(int.Parse(item.Id))).ToList();

        var maxDegreeOfParallelism = 32; // Set the maximum degree of parallelism
        using var semaphore = new SemaphoreSlim(maxDegreeOfParallelism);

        var tasks = items?.Select(async item =>
        {
            await semaphore.WaitAsync();
            try
            {
                await FetchAndAddRecipe(item, service);
            }
            finally
            {
                semaphore.Release();
            }
        });
        // var tasks = items?.Select(item => FetchAndAddRecipe(item, service));
        if (tasks != null) await Task.WhenAll(tasks);
    } while (responseResult?.SearchResults.NextUrl != null && fetchLimit > 1);
});

app.MapGet("/scrapecollection", async (string collectionSlug, bool fetchSinglePage, RecipeDataService service) =>
{
    try
    {
        string collectionName;
        string collectionDesc;
        
        var config = Configuration.Default.WithDefaultLoader();
        var address = "https://www.bbcgoodfood.com/recipes/collection/" + collectionSlug;
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync(address);
        collectionName = document.QuerySelector("h1.heading-1")?.TextContent;
        collectionDesc = document.QuerySelector("div.editor-content.mt-sm.pr-xxs.hidden-print p")?.TextContent;

        var collectionId = await service.InsertCollection(collectionName, collectionDesc, collectionSlug);
        
        var fetchLimit = fetchSinglePage ? 1 : 100;
        List<Item> items;
        List<int> itemIds;
        List<int> existingItems;
        CollectionResponse? responseResult = null;
        var firstFetch = true;
        do
        {
            if (firstFetch)
            {
                responseResult =
                    await httpClient.GetJsonAsync<CollectionResponse>(
                        bbcBaseAPI + "lists/posts/list/" + collectionSlug + "/items");
                firstFetch = false;
            }
            else
            {
                responseResult = await httpClient.GetJsonAsync<CollectionResponse>(responseResult?.NextUrl);
            }

            if (responseResult == null || responseResult?.Items.Count < 1)
                break;

            items = responseResult?.Items?.Select(item => new Item()
            {
                Id = item.Id,
                Url = item.Url,
                IsPremium = item.IsPremium,
            }).ToList();
            itemIds = items.ConvertAll(item => int.Parse(item.Id));
            existingItems = await service.GetExistingRecipeIds(itemIds);
            items = items.Where(item => !existingItems.Contains(int.Parse(item.Id))).ToList();

            var maxDegreeOfParallelism = 24; // Set the maximum degree of parallelism
            using var semaphore = new SemaphoreSlim(maxDegreeOfParallelism);

            var tasks = items?.Select(async item =>
            {
                await semaphore.WaitAsync();
                try
                {
                    await FetchAndAddRecipe(item, service);
                    await service.AddToCollectionRecipes(collectionId, Convert.ToInt32(item.Id));
                }
                finally
                {
                    semaphore.Release();
                }
            });

            if (tasks != null) await Task.WhenAll(tasks);
        } while (responseResult?.NextUrl != null && fetchLimit > 1);
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
        if (recipeData?.Props?.PageProps?.Schema != null)
        {
            var pageProps = recipeData.Props.PageProps;
            var contentData = pageProps?.Schema?.Name != null
                ? await GetContentApiResponse(Convert.ToInt32(item.Id), pageProps.Schema.Name)
                : null;
            await service.FetchOrAddRecipe(pageProps, contentData);
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

        var urlEncodedText = HttpUtility.UrlEncode(JsonSerializer.Serialize(apiRequestBody));

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