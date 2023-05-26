using bbc_scraper_api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace bbc_scraper_api.Services
{
    public class RecipeDataServiceOld
    {
        private readonly IMongoCollection<RecipeDataModel> _recipeCollection;
        private readonly IMongoCollection<CollectionModel> _collectionCollection;

        public RecipeDataServiceOld(
            IOptions<RecipeDatabaseSettings> recipeDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                recipeDatabaseSettings.Value.ConnectionString);

            var mongoDb = mongoClient.GetDatabase(
                recipeDatabaseSettings.Value.DatabaseName);

            _recipeCollection = mongoDb.GetCollection<RecipeDataModel>(
                recipeDatabaseSettings.Value.RecipeCollectionName);

            _collectionCollection = mongoDb.GetCollection<CollectionModel>(
                recipeDatabaseSettings.Value.CollectionCollectionName);
        }

        public async Task<bool> ItemExists(int id)
        {
            var res =  _recipeCollection.Find(x => x.Id == id).Limit(1).CountDocuments() > 0;
            return res;
        }

        public async Task<List<int>> CheckforMultipleItems(List<int> ids)
        {
            var existingIds = await _recipeCollection
                .Find(x => ids.Contains(x.Id))
                .Project(x => x.Id)
                .ToListAsync();

            return existingIds;
        }

        public async Task CreateAsync(RecipeDataModel newrecipe) =>
            await _recipeCollection.InsertOneAsync(newrecipe);

        public async Task CreateCollectionItemAsync(CollectionModel newcollection) =>
            await _collectionCollection.InsertOneAsync(newcollection);
    }
}
