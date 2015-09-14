using Microsoft.AspNet.Mvc;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi
{
    [Route("api/[controller]")]
    public class DemoController : Controller
    {
        [Route("")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Sun", "Mon", "Tue", "Wed", "Thr", "Fri", "Sat" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
    }
}