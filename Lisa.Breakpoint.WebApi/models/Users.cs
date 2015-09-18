namespace Lisa.Breakpoint.WebApi
{
    public class User
    {
        public int Id { get; internal set; }

        public string Username { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }

}
