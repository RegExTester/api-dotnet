using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using RegExTester.Api.DotNet.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RegExTester.Api.DotNet.Services;


public interface ITelemetryService
{
    Task SendTelemetryAsync(HttpRequest request, Input model, CancellationToken cancellationToken);
}

public class TelemetryService : ITelemetryService, IDisposable
{
    public static ItemRequestOptions ItemRequestOptions = new ItemRequestOptions { EnableContentResponseOnWrite = false };

    public static CosmosClient CosmosClient { get; private set; }
    public static Database CosmosDatabase { get; private set; }
    public static Container CosmosContainer { get; private set; }

    public TelemetryService(string cosmosConnectionString, string database, string container)
    {
        InitCosmos(cosmosConnectionString, database, container);
    }

    public async Task SendTelemetryAsync(HttpRequest request, Input model, CancellationToken cancellationToken)
    {
        if (CosmosClient is null || CosmosContainer is null)
            return;

        var item = new
        {
            id = Guid.NewGuid().ToString(),
            timestamp = DateTime.UtcNow.ToString("o"),
            host = request.Host.Value,
            useragent = request.Headers["User-Agent"],
            pattern = model.Pattern,
            text = model.Text,
            replace = model.Replace,
            options = model.Options.ToString()
        };

        await CosmosContainer.CreateItemAsync(item, new PartitionKey(item.timestamp), ItemRequestOptions, cancellationToken: cancellationToken);
    }

    private void InitCosmos(string cosmosConnectionString, string database, string container)
    {
        if (string.IsNullOrEmpty(cosmosConnectionString) || CosmosClient is not null)
            return;

        CosmosClient = new CosmosClient(cosmosConnectionString);
        CosmosDatabase = CosmosClient?.CreateDatabaseIfNotExistsAsync(database, ThroughputProperties.CreateManualThroughput(400)).Result.Database;
        CosmosContainer = CosmosDatabase?.CreateContainerIfNotExistsAsync(container, "/timestamp").Result.Container;
    }

    public void Dispose()
    {
        CosmosContainer = null;
        CosmosDatabase = null;
        CosmosClient?.Dispose();
    }
}
