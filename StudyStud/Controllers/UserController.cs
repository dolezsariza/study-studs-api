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

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfile(string userId)
        {
            var user = await _context.UserList.SingleOrDefaultAsync(user => user.Id == userId);
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

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateProfile(string userId, [FromBody]User user)
        {
            
            var nuser = await _context.UserList.SingleOrDefaultAsync(user => user.Id == userId);

            nuser.FirstName = user.FirstName;
            nuser.LastName = user.LastName;
            nuser.NickName = user.NickName;
            nuser.Introduction = user.Introduction;
            nuser.Interests = user.Interests;
            nuser.School = user.School;
            nuser.City = user.City;

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

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteProfile(string userId)
        {
            User user = await _context.UserList.SingleOrDefaultAsync(user => user.Id == userId);
            List<Topic> topicsOfUser = await _context.TopicList.Where(user => user.OwnerId == userId).ToListAsync();
            List<Post> postsOfUser = await _context.PostList.Where(user => user.OwnerId == userId).ToListAsync();
            
            foreach (Topic topic in topicsOfUser)
            {
                topic.OwnerId = null;
            }

            foreach (Post post in postsOfUser)
            {
                post.OwnerId = null;
            }

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

        [HttpGet("{userId}/topics")]
        public async Task<IActionResult> ListUsersTopics(string userId)
        {
            List<Topic> topicsOfUser = await _context.TopicList.Where(user => user.OwnerId == userId).ToListAsync();

            return Ok(topicsOfUser);
        }

        [HttpGet("{userId}/posts")]
        public async Task<IActionResult> ListUsersPosts(string userId)
        {
            List<Post> postsOfUser = await _context.PostList.Where(user => user.OwnerId == userId).ToListAsync();
            
            return Ok(postsOfUser);
        }

        [HttpGet("{userId}/{topicId}/posts")]
        public async Task<IActionResult> ListUsersPostsInTopic(string userId,int topicId)
        {
            List<Post> postsOfTopic = await _context.PostList.Where(post => post.TopicID == topicId && post.OwnerId == userId).ToListAsync();

            return Ok(postsOfTopic);
        }
    }
}
