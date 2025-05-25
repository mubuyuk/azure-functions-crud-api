using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace MyFunctionApp.Models
{
    public class Ballers
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("number")]
        public string Number { get; set; }

        [JsonPropertyName("team")]
        public string Team { get; set; }
    }
}
