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

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetProfile(string userName)
        {
            {
                var user = await _context.UserList.SingleOrDefaultAsync(user => user.UserName == userName);
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
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpPut("{userName}")]
        public async Task<IActionResult> UpdateProfile(string userName, [FromBody]User user)
        {
            try
            {
                var nuser = await _context.UserList.SingleOrDefaultAsync(user => user.UserName == userName);

                nuser.FirstName = user.FirstName;
                nuser.LastName = user.LastName;
                nuser.NickName = user.NickName;
                nuser.Introduction = user.Introduction;
                nuser.Interests = user.Interests;
                nuser.School = user.School;
                nuser.City = user.City;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return NotFound();
            }
           
        }

        [HttpDelete("{userName}")]
        public async Task<IActionResult> DeleteProfile(string userName)
        {
            try
            {
                User user = await _context.UserList.SingleOrDefaultAsync(user => user.UserName == userName);
                List<Topic> topicsOfUser = await _context.TopicList.Where(topic => topic.OwnerId == user.Id).ToListAsync();
                List<Post> postsOfUser = await _context.PostList.Where(post => post.OwnerId == user.Id).ToListAsync();
            
                foreach (Topic topic in topicsOfUser)
                {
                    topic.OwnerId = null;
                }

                foreach (Post post in postsOfUser)
                {
                    post.OwnerId = null;
                }

                _context.UserList.Remove(user);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest();
            }
        }

        [HttpGet("{userName}/topics")]
        public async Task<IActionResult> ListUsersTopics(string userName)
        {
            try
            {
                User user = await _context.UserList.SingleOrDefaultAsync(user => user.UserName == userName);
                List<Topic> topicsOfUser = await _context.TopicList.Where(topic => topic.OwnerId == user.Id).ToListAsync();

                return Ok(topicsOfUser);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpGet("{userName}/posts")]
        public async Task<IActionResult> ListUsersPosts(string userName)
        {
            try
            {
                User user = await _context.UserList.SingleOrDefaultAsync(user => user.UserName == userName);
                List<Post> postsOfUser = await _context.PostList.Where(post => post.OwnerId == user.Id).ToListAsync();

                return Ok(postsOfUser);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpGet("{userName}/{topicId}/posts")]
        public async Task<IActionResult> ListUsersPostsInTopic(string userName,int topicId)
        {
            try
            {
                User user = await _context.UserList.SingleOrDefaultAsync(user => user.UserName == userName);
                List<Post> postsOfTopic = await _context.PostList.Where(post => post.TopicID == topicId && post.OwnerId == user.Id).ToListAsync();

                return Ok(postsOfTopic);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }
    }
}
