using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleAPI.Models;

namespace SimpleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthContext _context;

        public AuthController(AuthContext context)
        {
            _context = context;
        }

        // GET: api/CheckToken
        [HttpPost]
        [Route("~/api/CheckToken")]
        public async Task<ActionResult<string>> PostCheckToken()
        {
            var headers = Request.Headers;
            bool isTokenValid = false;
            if (headers.ContainsKey("Authorization"))
                isTokenValid = await _context.UserList.AnyAsync(e => e.Token.ToString().Equals(headers["Authorization"]));
            
            return "Token " + (isTokenValid ? "valid": "invalid");
        }

        // GET: api/Auth
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUserList()
        {
            await _context.SaveChangesAsync();
            return await _context.UserList.ToListAsync();
        }

        // GET: api/Auth/user_name_1
        [HttpGet("{name}")]
        public async Task<ActionResult<User>> GetUser(string name)
        {
            var user = await _context.UserList.FirstOrDefaultAsync(e => e.Name == name);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        
        // POST: api/Auth
        [HttpPost]
        public async Task<ActionResult<string>> PostUser(LoginInfo loginInfo)
        {
            if (_context.UserList.Any(e => e.Name == loginInfo.Name))
            {
                return Conflict();
            }

            var user = new User()
                {
                    Id = _context.UserList.MaxAsync(e => e.Id).Id + 1,
                    Name = loginInfo.Name, 
                    Password = loginInfo.Password, 
                    Token = Guid.NewGuid()
                };
            _context.UserList.Add(user);
            await _context.SaveChangesAsync();

            return $"{{\"token\" : \"{user.Token}\"}}";
        }
    }
}
