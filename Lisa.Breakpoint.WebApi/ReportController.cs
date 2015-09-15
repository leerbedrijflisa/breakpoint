using Lisa.Breakpoint.WebApi.Models;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        [Route("")]
        [HttpGet]
        public List<Report> Get()
        {
            try
            {
                return  new List<Report> {
                    new Report { Id = 1, Email = "Series1", Date = new DateTime(2001,1,1)},
                    new Report { Id = 2, Email = "Series2", Date = new DateTime(2002,1,30)},
                    new Report { Id = 10, Email = "Series10", Date = new DateTime(2001,5,5)}
                };

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