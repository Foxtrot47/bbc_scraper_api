using bbc_scraper_api.Models;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


app.MapGet("/search", async (string query) => {
    var response = await httpClient.GetJsonAsync<Result>(bbcBaseAPI + "search?search=" + query + "&limit=9999");
    var finalRes = new List<List<ContentAPIResponse>>();
    if (response != null && response.SearchResults.TotalItems > 0)
    {
        List<Item> items = new(response.SearchResults.Items);

        foreach(var item in items)
        {
            var task = Task.Run(async () => await GetRecipeData(items[0]));
            finalRes.Add(task.Result);
        }
    }
    return Results.Json(finalRes);
});

static async Task<List<ContentAPIResponse>> GetRecipeData(Item item)
{
    var client = new RestClient(bbbcContentAPI);
    var apiRequestBody = new ContentAPIRequest
    {
        siteKey = "bbcgoodfood",
        searchTerm = "carrot-biriyani",
        postId = Convert.ToInt32(item.Id),
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

