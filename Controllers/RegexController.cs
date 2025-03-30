using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using RegExTester.Api.DotNet.Models;
using RegExTester.Api.DotNet.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RegExTester.Api.DotNet.Controllers
{
    [Route("api/regex")]
    public class RegExController : Controller
    {
        public IRegExProcessor RegExProcessor { get; set; }
        public IConfiguration Configuration { get; set; }

        public RegExController(IRegExProcessor regExProcessor, IConfiguration configuration)
        {
            this.RegExProcessor = regExProcessor;
            this.Configuration = configuration;
        }

        // POST api/regex
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Input model, CancellationToken cancellationToken)
        {
            var result = RegExProcessor.Matches(model.Pattern, model.Text, model.Replace, model.Options);
            await AddTelemetryAsync(model, cancellationToken);
            return Json(result);
        }

        private async Task AddTelemetryAsync(Input model, CancellationToken cancellationToken)
        {
            var cosmosConnectionString = Configuration["Cosmos:ConnectionString"];
            var cosmosDb = Configuration["Cosmos:Database"];
            var cosmosContainer = Configuration["Cosmos:Container"];
            if (string.IsNullOrEmpty(cosmosConnectionString) || string.IsNullOrEmpty(cosmosDb) || string.IsNullOrEmpty(cosmosContainer))
                return;

            var client = new CosmosClient(cosmosConnectionString);
            var database = client.GetDatabase(cosmosDb);
            var container = database.GetContainer(cosmosContainer);

            await container.CreateItemAsync(new
            {
                id = Guid.NewGuid(),
                timestamp = DateTime.UtcNow,
                host = Request.Host.Value,
                useragent = Request.Headers["User-Agent"],
                pattern = model.Pattern,
                text = model.Text,
                replace = model.Replace,
                options = model.Options.ToString()
            }, cancellationToken: cancellationToken);
        }
    }
}
