using Microsoft.AspNet.Mvc;

namespace Lisa.Breakpoint.WebApi
{
    [Route("")]
    public class VersionController : Controller
    {
        [Route("version")]
        [HttpGet]
        public string Get()
        {
            string VersionNumber = "v2";

            return VersionNumber;
        }
    }
}