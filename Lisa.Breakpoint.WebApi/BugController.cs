using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi
{
    [Route("bugs")]
    public class BugController
    {
        static SqlDatabase _db = new SqlDatabase();

        [HttpGet]
        public object Get()
        {
            try
            {
                IList<Bug> bugs = _db.GetAllBugs();
                return bugs;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("{id}")]
        public object getBug(string id)
        {
            try
            {
                var bug = _db.GetBugById(id);

                return bug;
            }
            catch (Exception)
            {
                return "error";
            }
        }

        [HttpGet]
        [Route("insert")]
        public void insert(string id)
        {
            try
            {
                Bug bug = new Bug
                {
                    Title = "inserted bug",
                    Description = "This bug is inserted with a query",
                    Status = "not fixed"
                };

                _db.insertBug(bug);
            }
            catch (Exception)
            {
                // return "error";
            }
        }

        [HttpGet]
        [Route("update/{id}")]
        public void update(string id)
        {
            try
            {
                Bug newBug = new Bug
                {
                    Title = "updated bug",
                    Description = "This bug is updated with a query",
                    Status = "fixed"
                };

                _db.updateBug(id, newBug);
            }
            catch (Exception)
            {
                // return "error";
            }
        }

        [HttpGet]
        [Route("delete/{id}")]
        public void delete(string id)
        {
            try
            {
                _db.deleteBug(id);
            }
            catch (Exception)
            {
                // return "error";
            }
        }
    }
}
