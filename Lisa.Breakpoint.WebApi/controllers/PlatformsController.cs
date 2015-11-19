using Lisa.Breakpoint.WebApi.database;
using Microsoft.AspNet.Mvc;

namespace Lisa.Breakpoint.WebApi.controllers
{
    [Route("platforms")]
    public class PlatformController
    {
        public PlatformController(RavenDB db)
        {
            _db = db;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            return new HttpOkObjectResult(_db.GetPlatforms());
        }

        private readonly RavenDB _db;
    }
}
