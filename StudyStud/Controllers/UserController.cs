using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using StudyStud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyStud.Controllers
{
    [Route("profile")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly StudyDbContext _context;

        public UserController(StudyDbContext context)
        {
            _context = context;
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {
            var user = await _context.UserList.SingleOrDefaultAsync(user => user.UserName == username);
            Profile profile = new Profile()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                NickName = user.NickName,
                Introduction = user.Introduction,
                Interests = user.Interests,
                School = user.School,
                City = user.City
            };

            return Ok(profile);
        }

        [HttpPut("{username}")]
        public async Task<IActionResult> UpdateProfile(string username, [FromBody]Object json)
        {
            JObject jObject = JObject.Parse(json.ToString());
            var user = await _context.UserList.SingleOrDefaultAsync(user => user.UserName == username);

            user.FirstName = jObject.GetValue("FirstName").ToString();
            user.LastName = jObject.GetValue("LastName").ToString();
            user.NickName = jObject.GetValue("NickName").ToString();
            user.Introduction = jObject.GetValue("Introduction").ToString();
            user.Interests = jObject.GetValue("Interests").ToString();
            user.School = jObject.GetValue("School").ToString();
            user.City = jObject.GetValue("City").ToString();

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return StatusCode(204);
            }
           
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteProfile(string username)
        {
            var user = await _context.UserList.SingleOrDefaultAsync(user => user.UserName == username);
            _context.UserList.Remove(user);
            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return StatusCode(204);
            }
        }
    }
}
