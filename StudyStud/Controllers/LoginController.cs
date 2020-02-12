using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyStud.Models;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace StudyStud.Controllers
{
    [Route("login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly StudyDbContext _context;

        private readonly SignInManager<User> _signInManager;

        public LoginController(SignInManager<User> signInManager, StudyDbContext context)
        {
            _context = context;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<HttpStatusCode> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                   login.Username, login.Password, false, false);

                if (result.Succeeded)
                {
                    return HttpStatusCode.OK;
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return HttpStatusCode.Unauthorized;
        }
    }
}