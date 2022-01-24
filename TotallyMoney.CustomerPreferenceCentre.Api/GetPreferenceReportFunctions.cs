namespace TotallyMoney.CustomerPreferenceCentre.Api;

public class GetPreferenceReportFunctions
{
    [FunctionName("GetPreferenceReportFunction")]
    [OpenApiOperation(operationId: "GetPreferenceReportFunction", tags: new[] { "Preferences" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiRequestBody("application/json", typeof(object[]), Required = true, Description = "The customer preferences")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(CustomerPreferenceReportItem[]), Description = "The OK response")]
    public async Task<IActionResult> GetPreferenceReportFunction(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        ILogger logger)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var preferences = JsonConvert.DeserializeObject<CustomerPreference[]>(requestBody, new PreferenceConverter());
        var report = GenerateReport(preferences);
        return new OkObjectResult(report);
    }

    private CustomerPreferenceReportItem[] GenerateReport(CustomerPreference[] preferences)
    {
        var everyDay = new HashSet<string>();
        var specificDate = new Dictionary<int, HashSet<string>>();
        for (var d = 1; d <= 31; d++)
        {
            specificDate[d] = new HashSet<string>();
        }
        var daysOfWeek = new Dictionary<DayOfWeek, HashSet<string>>();
        foreach (var d in Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>())
        {
            daysOfWeek[d] = new HashSet<string>();
        }

        foreach (var preference in preferences)
        {
            preference.Preference.Switch(
                i =>
                {
                    specificDate[i].Add(preference.Name);
                },
                ds =>
                {
                    foreach (var d in ds)
                    {
                        daysOfWeek[d].Add(preference.Name);
                    }
                },
                e =>
                {
                    if (e)
                    {
                        everyDay.Add(preference.Name);
                    }
                }
            );
        }

        return Enumerable.Range(1, 90)
            .Select(i => DateTime.Today.AddDays(i))
            .Select(d => new CustomerPreferenceReportItem
            {
                Date = d,
                Customers = everyDay.Union(specificDate[d.Day]).Union(daysOfWeek[d.DayOfWeek]).ToArray(),
            })
            .ToArray();
    }
}

