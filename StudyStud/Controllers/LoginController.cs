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
        private readonly SignInManager<User> _signInManager;

        public LoginController(StudyDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            var user = await _userManager.FindByNameAsync(login.Username);
            if (user == null)
            {
                return BadRequest("Wrong username or password");
            }
            var result = await _userManager.CheckPasswordAsync(
                user, login.Password);

            if (result)
            {
                await _signInManager.SignInAsync(user, isPersistent: true);
                return Ok(User.Claims.Select(c => c.Value));
            }
            return BadRequest("Wrong username or password");
        }
    }
}
