using Microsoft.AspNet.Mvc;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi
{
    [Route("reports")]
    public class ReportController
    {
        static RavenDB _db = new RavenDB();

        [HttpGet]
        public IList<Report> Get()
        {
            return _db.getAll();
        }

        [HttpGet]
        [Route("{id}")]
        public Report get(int id)
        {
            return _db.get(id);
        }

        [HttpGet]
        [Route("insert")]
        public Report insert(int id)
        {
            return _db.insert();
        }

        [HttpGet]
        [Route("update/{id}")]
        public Report update(int id)
        {
            return _db.update(id);
        }

        [HttpGet]
        [Route("delete/{id}")]
        public void delete(int id)
        {
            _db.delete(id);
        }
    }
}