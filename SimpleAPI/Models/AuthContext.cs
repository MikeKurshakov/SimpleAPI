using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SimpleAPI.Models
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options)
            : base(options)
        {
            if (!UserList.Any())
            {
                for (int i = 1; i < 5; i++)
                {
                    var user = new User()
                    {
                        Id = i,
                        Name = $"user_name_{i}",
                        Password = $"user_password_{i}",
                        Token = Guid.NewGuid()
                    };
                    UserList.Add(user);
                }
            }
        }

        public DbSet<User> UserList { get; set; }
    }
}