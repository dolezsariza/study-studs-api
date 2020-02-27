using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyStud.Models;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;

namespace StudyStud.Controllers
{
    [Route("login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly StudyDbContext _context;
        private readonly UserManager<User> _userManager;

        public LoginController(StudyDbContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            var user = await _userManager.FindByNameAsync(login.Username);
            if(user == null)
            {
                return BadRequest("Wrong username or password");
            }
            var result = await _userManager.CheckPasswordAsync(
                user, login.Password);

            if (result)
            {
                var claims = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id),
                    new Claim("username", user.UserName),
                    new Claim("email", user.Email)
                });
                return Ok(claims.Claims.Select(c=>c.Value));
            }
            return BadRequest("Wrong username or password");
        }
    }
}
