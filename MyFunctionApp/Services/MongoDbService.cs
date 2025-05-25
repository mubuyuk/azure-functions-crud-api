using MongoDB.Driver;
using MyFunctionApp.Models;

namespace MyFunctionApp.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase db;

        public MongoDbService(string connectionString, string database)
        {
            var client = new MongoClient(connectionString);
            db = client.GetDatabase(database);
        }

        public async Task<Ballers> AddBallerAsync(string collectionName, Ballers baller)
        {
            var collection = db.GetCollection<Ballers>(collectionName);

            await collection.InsertOneAsync(baller);
            return baller;
        }

        public async Task<List<Ballers>> GetAllBallersAsync(string collectionName)
        {
            var collection = db.GetCollection<Ballers>(collectionName);
            return await collection.Find(_ => true).ToListAsync();
        }

        public async Task<Ballers?> UpdateBallerAsync(string collectionName, string id, Ballers updated)
        {
            var collection = db.GetCollection<Ballers>(collectionName);
            updated.Id = id;

            var result = await collection.ReplaceOneAsync(
                b => b.Id == id,
                updated);

            return result.MatchedCount == 0 ? null : updated;
        }


        public async Task<bool> DeleteBallerAsync(string collectionName, string id)
        {
            var collection = db.GetCollection<Ballers>(collectionName);
            var result = await collection.DeleteOneAsync(b => b.Id == id);
            return result.DeletedCount > 0;
        }

    }
}
