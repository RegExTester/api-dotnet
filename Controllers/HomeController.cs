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
        [ResponseCache(Duration = 60*60*24)] // 1d
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
