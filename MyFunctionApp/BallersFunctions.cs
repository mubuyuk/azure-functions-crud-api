using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MyFunctionApp.Services;
using MyFunctionApp.Models;
using System.Text.Json;

namespace MyFunctionApp
{
    public class BallersFunctions
    {
        private readonly ILogger<BallersFunctions> _logger;
        private readonly MongoDbService _mongoService;

        public BallersFunctions(ILogger<BallersFunctions> logger, MongoDbService mongoService)
        {
            _logger = logger;
            _mongoService = mongoService;
        }

        /// <summary>
        /// Skapar en ny baller.
        /// POST-anrop till /api/baller med JSON-body som innehåller name, number och team.
        /// Exempel-body:
        /// {
        ///   "name": "Michael Jordan",
        ///   "number": "23",
        ///   "team": "Chicago Bulls"
        /// }
        /// </summary>
        [Function("CreateBaller")]
        public async Task CreateBaller(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "baller")] HttpRequest req)
        {
            var baller = await JsonSerializer.DeserializeAsync<Ballers>(
                req.Body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (baller == null)
            {
                req.HttpContext.Response.StatusCode = 400;
                await req.HttpContext.Response.WriteAsJsonAsync(new { error = "Invalid JSON" });
                return;
            }

            var newBaller = await _mongoService.AddBallerAsync("ballerCollection", baller);
            await req.HttpContext.Response.WriteAsJsonAsync(newBaller);
        }

        /// <summary>
        /// Hämtar alla ballers.
        /// GET-anrop till /api/ballers returnerar en lista med alla objekt i ballerCollection.
        /// </summary>
        [Function("GetBallers")]
        public async Task GetBallers(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "ballers")] HttpRequest req)
        {
            var allBallers = await _mongoService.GetAllBallersAsync("ballerCollection");
            await req.HttpContext.Response.WriteAsJsonAsync(allBallers);
        }

        /// <summary>
        /// Uppdaterar en baller.
        /// PUT-anrop till /api/ballers/{id} med komplett JSON-body.
        /// OBS Ersätter hela objektet. Alla fält måste vara med i bodyn.
        /// </summary>
        [Function("UpdateBaller")]
        public async Task UpdateBaller(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "ballers/{id}")] HttpRequest req, string id)
        {
            var updatedBaller = await JsonSerializer.DeserializeAsync<Ballers>(
                req.Body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (updatedBaller == null)
            {
                req.HttpContext.Response.StatusCode = 400;
                await req.HttpContext.Response.WriteAsJsonAsync(new { error = "Invalid JSON" });
                return;
            }

            var result = await _mongoService.UpdateBallerAsync("ballerCollection", id, updatedBaller);

            if (result == null)
            {
                req.HttpContext.Response.StatusCode = 404;
                await req.HttpContext.Response.WriteAsJsonAsync(new { error = "Baller not found" });
                return;
            }

            await req.HttpContext.Response.WriteAsJsonAsync(result);
        }

        /// <summary>
        /// Raderar en baller.
        /// DELETE-anrop till /api/ballers/{id} tar bort baller med id.
        /// Returnerar meddelande om det lyckades eller 404 om inget hittades.
        /// </summary>
        [Function("DeleteBaller")]
        public async Task DeleteBaller(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "ballers/{id}")] HttpRequest req, string id)
        {
            var deleted = await _mongoService.DeleteBallerAsync("ballerCollection", id);

            if (!deleted)
            {
                req.HttpContext.Response.StatusCode = 404;
                await req.HttpContext.Response.WriteAsJsonAsync(new { error = "Baller not found" });
                return;
            }

            await req.HttpContext.Response.WriteAsJsonAsync(new { message = "Baller deleted" });
        }
    }
}
