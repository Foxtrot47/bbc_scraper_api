using RestSharp;

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
    if (response != null && response.SearchResults.TotalItems > 0)
    {
        List<Item> items = new List<Item>(response.SearchResults.Items);

        //if (response.SearchResults.TotalItems > response.SearchResults.Limit)
        //{
            //const int pageCount = Math.Ceiling(Convert.ToDecimal(response.SearchResults.TotalItems / response.SearchResults.Limit - 1);
            //for (var responseLoopIndex = 0; responseLoopIndex < pageCount; responseLoopIndex++)
            //{
            //    var result = await httpClient.GetJsonAsync<Result>(bbcBaseAPI + "search?search=" + query)
            //}
        //}
    }
    return response;
});

app.Run();

