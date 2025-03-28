using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace RegExTester.Api.DotNet.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        // GET /
        [HttpGet]
        public RedirectResult Get()
        {
            return Redirect("https://regextester.github.io/");
        }

        // GET api/version
        [HttpGet]
        [Route("/api/version")]
        public ActionResult Version()
        {
            return Json(new {
                #if DEBUG
                debug = 1,
                #endif
                os = RuntimeInformation.OSDescription,
                framework = RuntimeInformation.FrameworkDescription
            });
        }
    }
}
