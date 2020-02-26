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
    [Route("topics")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly StudyDbContext _context;

        public TopicController(StudyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTopics()
        {
            try
            {
                List<Topic> topics = await _context.TopicList.ToListAsync();
                return Ok(topics);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.StackTrace);
                return StatusCode(204);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTopic(Topic topic)
        {
            try
            {
                User owner = await _context.UserList.SingleOrDefaultAsync(user => user.Id == topic.OwnerId);
                topic.OwnerName = owner.UserName;
                _context.TopicList.Add(topic);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return StatusCode(406);
            }
        }

        [HttpGet("{topicId}")]
        public async Task<IActionResult> GetTopic(int topicId)
        {
            try
            {
                Topic topic = await _context.TopicList.SingleOrDefaultAsync(topic => topic.Id == topicId);
                topic.Posts = await _context.PostList.Where(post => post.TopicID == topicId).ToListAsync();
                return Ok(topic);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return StatusCode(204);
            }
        }

        [HttpPost("{topicId}")]
        public async Task<IActionResult> AddPostToTopic(int topicId, Post post)
        {
            try
            {
                User owner = await _context.UserList.SingleOrDefaultAsync(user => user.Id == post.OwnerId);
                post.OwnerName = owner.UserName;
                post.TopicID = topicId;
                _context.PostList.Add(post);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return StatusCode(406);
            }
        }

        [HttpDelete("{topicId}")]
        public async Task<IActionResult> DeleteTopic(int topicId, [FromBody]JObject user)
        {
            string userId = user.GetValue("UserId").ToString();
            try
            {
                Topic topic = await _context.TopicList.SingleOrDefaultAsync(topic => topic.Id == topicId);

                if (topic.OwnerId == userId)
                {
                    List<Post> posts = await _context.PostList.Where(post => post.TopicID == topicId).ToListAsync();
                    _context.PostList.RemoveRange(posts);
                    _context.TopicList.Remove(topic);
                    await _context.SaveChangesAsync();
                    return Ok();
                } else
                {
                    return Unauthorized();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return StatusCode(406);
            }
        }

        [HttpDelete("{topicId}/{postId}")]
        public async Task<IActionResult> DeletePost(int postId, [FromBody]JObject user)
        {
            string userId = user.GetValue("UserId").ToString();

            try
            {
                Post post = await _context.PostList.SingleOrDefaultAsync(post => post.Id == postId);

                if (post.OwnerId == userId)
                {
                    _context.PostList.Remove(post);
                    await _context.SaveChangesAsync();
                    return Ok();
                } else
                {
                    return Unauthorized();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return StatusCode(406);
            }
        }
    }
}
