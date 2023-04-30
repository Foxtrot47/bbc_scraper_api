using bbc_scraper_api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace bbc_scraper_api.Services
{
    public class RecipeDataService
    {
        private readonly IMongoCollection<RecipeDataModel> _recipeCollection;

        public RecipeDataService(
            IOptions<RecipeDatabaseSettings> recipeDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                recipeDatabaseSettings.Value.ConnectionString);

            var mongoDb = mongoClient.GetDatabase(
                recipeDatabaseSettings.Value.DatabaseName);

            _recipeCollection = mongoDb.GetCollection<RecipeDataModel>(
                recipeDatabaseSettings.Value.RecipeCollectionName);
        }

        public async Task<bool> ItemExists(int id)
        {
            var res =  _recipeCollection.Find(x => x.Id == id).Limit(1).CountDocuments() > 0;
            return res;
        }

        public async Task CreateAsync(RecipeDataModel newrecipe) =>
            await _recipeCollection.InsertOneAsync(newrecipe);
    }
}
