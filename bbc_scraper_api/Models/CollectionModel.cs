using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace bbc_scraper_api.Models;

[BsonIgnoreExtraElements]
public class CollectionModel
{
    public ObjectId Id { get; set; }
    
    [BsonElement("collectionName")]
    public string CollectionName { get; set; }

    [BsonElement("collectionSlug")] 
    public string CollectionSlug { get; set; }
    
    [BsonElement("dataAdded")]
    public DateTime DateAdded { get; set; }

    [BsonElement("recipeName")]
    public string? RecipeNameName { get; set; }
    
    [BsonElement("recipeUrl")]
    public string? Url { get; set; }
    
    [BsonElement("recipeImage")]
    public Image? Image { get; set; }
    
    [BsonElement("rating")] 
    public RecipeDataRating Rating { get; set; }
}