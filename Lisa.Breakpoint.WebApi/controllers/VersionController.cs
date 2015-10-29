using Microsoft.AspNet.Mvc;

namespace Lisa.Breakpoint.WebApi
{
    [Route("versions")]
    public class VersionController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "v2";
        }
    }
}