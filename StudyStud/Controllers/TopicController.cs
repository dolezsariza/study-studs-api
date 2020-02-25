using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                await _context.TopicList.AddAsync(topic);
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
                Topic topic = await _context.TopicList.SingleOrDefaultAsync(topic => topic.Id == topicId);
                post.Topic = topic;
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
        public async Task<IActionResult> DeleteTopic(int topicId, string userId)
        {
            try
            {
                Topic topic = await _context.TopicList.SingleOrDefaultAsync(topic => topic.Id == topicId);

                if (topic.Owner.Id == userId)
                {
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
        public async Task<IActionResult> DeletePost(int postId, string userId)
        {
            try
            {
                Post post = await _context.PostList.SingleOrDefaultAsync(post => post.Id == postId);

                if (post.Owner.Id == userId)
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
