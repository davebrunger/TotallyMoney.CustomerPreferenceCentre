namespace TotallyMoney.CustomerPreferenceCentre.Api;

internal class PreferenceFunctions
{
    private static readonly string databaseId = Environment.GetEnvironmentVariable("CosmosDbDatabaseId")!;

    [FunctionName("SavePreference")]
    [OpenApiOperation(operationId: "SaveName", tags: new[] { "Names" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiRequestBody("application/json", typeof(object), Required = true, Description = "The customer preference")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
    public async Task<IActionResult> SaveName(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "names")] HttpRequest req,
        [CosmosDB(ConnectionStringSetting = "CosmosDbConnectionString", CreateIfNotExists = true)] DocumentClient client,
        ILogger logger)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var preference = JsonConvert.DeserializeObject<CustomerPreference>(requestBody, new PreferenceConverter());

        await client.CreateAsync(databaseId, CustomerPreference.CollectionId, preference);

        return new OkResult();
    }
}
