using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using StudyStud.Models;
using StudyStud.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
                List<Topic> topics = await _context.TopicList.OrderByDescending(t => t.Date).ToListAsync();
                return Ok(topics);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.StackTrace);
                return NoContent();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTopic(TopicPostRequest topicRequest)
        {
            try
            {
                Topic topic = new Topic() { Description=topicRequest.Description, Title= topicRequest.Title };
                var userName = User.Identity.Name;
                var ownerId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
                topic.OwnerId = ownerId.Value;
                topic.OwnerName = userName;
                _context.TopicList.Add(topic);
                await _context.SaveChangesAsync();
                return Created("","");
            }
            catch (DbUpdateException e)
            {
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
                return NoContent();
            }
        }

        [HttpPost("{topicId}")]
        public async Task<IActionResult> AddPostToTopic(int topicId, Post post)
        {
            try
            {
               HashSet<bool> contains = new HashSet<bool>(2);
               await _context.TopicList.ForEachAsync(t => contains.Add(t.Id == topicId));
               if (!contains.Contains(true))
                    throw new Exception("Wrong topic Id");
                post.OwnerName = User.Identity.Name;
                post.OwnerId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
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
        public async Task<IActionResult> DeleteTopic(int topicId, [FromBody]Object user)
        {
            try
            {
                string userId = JObject.Parse(user.ToString()).GetValue("UserId").ToString();
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
        public async Task<IActionResult> DeletePost(int postId, [FromBody]Object user)
        {
            try
            {
                string userId = JObject.Parse(user.ToString()).GetValue("UserId").ToString();
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

        [HttpGet("{topicId}/files")]
        public async Task<IActionResult> GetFiles (int topicId)
        {
            var files = await _context.FileList.Where(file => file.TopicId == topicId).Select(f => new TopicGetFiles { FileName = f.FileName, Id = f.Id}).ToListAsync();
            return Ok(files);
        }

    }
}
