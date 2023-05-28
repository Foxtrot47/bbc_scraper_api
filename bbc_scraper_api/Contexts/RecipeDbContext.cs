// using Npgsql;
// using System.Data;
//
// namespace bbc_scraper_api.Contexts;
//
// public class RecipeDbContext
// {
//     private readonly string _connectionString;
//
//     public RecipeDbContext(string connectionString)
//     {
//         _connectionString = connectionString;
//     }
//
//     public IDbConnection Connection
//     {
//         get
//         {
//             return new NpgsqlConnection(_connectionString);
//         }
//     }
//     
// }