
# Azure Functions API – Ballers

Detta projekt är ett enkelt serverless REST API byggt med Azure Functions och MongoDB (Cosmos DB). API:t hanterar resurser kallade "Ballers" med fälten namn, nummer och lag – och stödjer fulla CRUD-operationer (Create, Read, Update, Delete).

## Endpoints

| Metod  | Route               | Beskrivning                     |
|--------|---------------------|----------------------------------|
| POST   | `/api/baller`       | Skapa en ny baller               |
| GET    | `/api/ballers`      | Hämta alla ballers               |
| PUT    | `/api/ballers/{id}` | Uppdatera baller med angivet id |
| DELETE | `/api/ballers/{id}` | Radera baller med angivet id    |

## Exempel på JSON-body (för POST/PUT)

```json
{
  "name": "Michael Jordan",
  "number": "23",
  "team": "Chicago Bulls"
}
```

## Användning och testning

### Base URL
```
https://azure-functions-crud-api.azurewebsites.net/api
```

### Autentisering

Alla anrop kräver en Function Key som skickas som queryparameter:

```
?code=<din_function_key>
```

Exempel:
```
GET /api/ballers?code=abc123xyz==
```

## Säkerhet

API:t är skyddat med `AuthorizationLevel.Function`, vilket innebär att endast klienter med giltig nyckel kan göra anrop.

## Databas

Datat lagras i en Azure Cosmos DB-instans med MongoDB API. Anslutningen hanteras via `local.settings.json`.

## Testning

API:t har testats med Postman genom att skicka anrop till ovanstående endpoints med giltig Function Key.
