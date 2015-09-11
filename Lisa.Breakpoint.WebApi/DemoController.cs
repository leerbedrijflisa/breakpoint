namespace Lisa.Breakpoint.WebApi
{
    public class DemoController
    {
        public string[] Index()
        {
            string[] days = { "Sun", "Mon", "Tue", "Wed", "Thr", "Fri", "Sat"};

            return days;
        }
    }
}