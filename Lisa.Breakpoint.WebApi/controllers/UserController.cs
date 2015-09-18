using Microsoft.AspNet.Mvc;

namespace Lisa.Breakpoint.WebApi.controllers
{
    [Route("users")]
    public class UserController
    {
        static User _user = new User();

        public User inser()
        {
            User user = new User
            {
                Username = "Henk1",
                FullName = "HenkHenksen",
                Role = "User"
            }
        }
    }
}
