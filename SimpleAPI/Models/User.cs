using System;

namespace SimpleAPI.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public Guid Token { get; set; }
    }

    public class LoginInfo
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
