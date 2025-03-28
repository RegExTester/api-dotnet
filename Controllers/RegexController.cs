using Microsoft.AspNetCore.Mvc;
using RegExTester.Api.DotNet.Models;
using RegExTester.Api.DotNet.Services;

namespace RegExTester.Api.DotNet.Controllers
{
    [Route("api/regex")]
    public class RegExController : Controller
    {
        public IRegExProcessor RegExProcessor { get; set; }

        public RegExController(IRegExProcessor regExProcessor)
        {
            this.RegExProcessor = regExProcessor;
        }

        // POST api/regex
        [HttpPost]
        public ActionResult Post([FromBody] Input model)
        {
            var processor = new RegExProcessor();
            var result = RegExProcessor.Matches(model.Pattern, model.Text, model.Replace, model.Options);
            return Json(result);
        }
    }
}
