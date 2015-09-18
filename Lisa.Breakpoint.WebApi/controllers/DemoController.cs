using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi
{
    [Route("")]
    public class DemoController : Controller
    {
        [Route("")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            try
            {
                return new string[] { "Sun", "Mon", "Tue", "Wed", "Thr", "Fri", "Sat" };
            }
            catch (Exception)
            {
                return null;
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
    }
}