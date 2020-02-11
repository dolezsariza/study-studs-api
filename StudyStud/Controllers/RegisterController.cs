using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using StudyStud.Models;

namespace StudyStud.Controllers
{
    [Route("/register")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        public RegisterController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<HttpStatusCode> Post([FromBody]Object json)
        {
            JObject jObject = JObject.Parse(json.ToString());
            var user = new User { UserName = jObject.GetValue("userName").ToString(), Email = jObject.GetValue("email").ToString() };
            var result = await _userManager.CreateAsync(user, jObject.GetValue("password").ToString());
            if (result.Succeeded)
            {
                return HttpStatusCode.Created;
            }
            return HttpStatusCode.Conflict;
        }
    }
}