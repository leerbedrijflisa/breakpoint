using Microsoft.AspNet.Mvc;

namespace Lisa.Breakpoint.WebApi
{
    [Route("api/[controller]")]
    public class DemoController : Controller
    {
        [Route("")]
        public string[] GetAll()
        {
            string[] days = { "Sun", "Mon", "Tue", "Wed", "Thr", "Fri", "Sat" };

            return days;
        }
    }
}