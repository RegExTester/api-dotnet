using Microsoft.AspNetCore.Mvc;
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
        public ITelemetryService TelemetryService { get; set; }

        public RegExController(IRegExProcessor regExProcessor, ITelemetryService telemetryService)
        {
            this.RegExProcessor = regExProcessor;
            this.TelemetryService = telemetryService;
        }

        // POST api/regex
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Input model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = RegExProcessor.Matches(model.Pattern, model.Text, model.Replace, model.Options);
            await TelemetryService.SendTelemetryAsync(Request, model, cancellationToken);
            return Json(result);
        }
    }
}
