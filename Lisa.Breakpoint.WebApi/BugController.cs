using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi
{
    [Route("bugs")]
    public class BugController
    {
        static RavenDB _db = new RavenDB();

        [HttpGet]
        public object Get()
        {
            return _db;
        }

        [HttpGet]
        [Route("{id}")]
        public void getBug(string id)
        {
            
        }

        [HttpGet]
        [Route("insert")]
        public void insert(string id)
        {
            
        }

        [HttpGet]
        [Route("update/{id}")]
        public void update(string id)
        {
            
        }

        [HttpGet]
        [Route("delete/{id}")]
        public void delete(string id)
        {
            
        }
    }
}