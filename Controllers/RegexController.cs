using Microsoft.AspNetCore.Mvc;
using RegExTester.Api.DotNet.Models;
using RegExTester.Api.DotNet.Services;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RegExTester.Api.DotNet.Controllers
{
    [Route("api/regex")]
    public class RegExController : Controller
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Converters = { new JsonStringEnumConverter() }
        };

        public IRegExProcessor RegExProcessor { get; set; }

        public RegExController(IRegExProcessor regExProcessor)
        {
            this.RegExProcessor = regExProcessor;
        }

        // POST api/regex
        [HttpPost]
        public ActionResult Post([FromBody] Input model)
        {
            Activity.Current?.AddTag(nameof(model), JsonSerializer.Serialize(model, JsonOptions));

            var result = RegExProcessor.Matches(model.Pattern, model.Text, model.Replace, model.Options);
            return Json(result);
        }
    }
}
