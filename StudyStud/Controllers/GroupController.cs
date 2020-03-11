using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyStud.Models;

namespace StudyStud.Controllers
{
    [Route("groups")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly StudyDbContext _context;

        public GroupController(StudyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllGroups()
        {
            var groups = await _context.GroupList.ToListAsync();
            if(groups != null)
            {
                return Ok(groups);
            }

            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetGroup(int id)
        {
            var group = await _context.GroupList.FindAsync(id);

            if (group == null)
            {
                return NotFound();
            }

            return Ok(group);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyGroup(int id,[FromBody]Group modifiedGroup)
        {
            var group = await _context.GroupList.SingleOrDefaultAsync(g => g.Id == id);
            if (group == null)
            {
                return BadRequest();
            }

            group.Title = modifiedGroup.Title;
            group.Description = group.Description;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Group>> AddGroup(Group group)
        {
            _context.GroupList.Add(group);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetGroup", new { id = group.Id }, group);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGroup(int id)
        {
            var group = await _context.GroupList.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            _context.GroupList.Remove(group);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{id}/join")]
        public async Task<ActionResult> JoinToGroup(string userName, int GroupId)
        {
            try
            {
                var user = await _context.UserList.SingleOrDefaultAsync(u => u.UserName == userName);
                var group = await _context.GroupList.SingleOrDefaultAsync(g => g.Id == GroupId);
                if (user == null || group == null)
                {
                    return NotFound();
                }
                GroupUser groupUser = new GroupUser { GroupId = group.Id, Group = group, User = user, UserId = user.Id };
                user.GroupUsers.Add(groupUser);
                group.GroupUsers.Add(groupUser);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {

                return StatusCode(505);
            }
        }

        [HttpPost("{id}/addtopic")]
        public async Task<ActionResult> AddTopicToGroup(int groupId, [FromBody]Topic ntopic)
        {
            var topic = await _context.TopicList.SingleOrDefaultAsync(t => t.Id == ntopic.Id);
            var group = await _context.GroupList.SingleOrDefaultAsync(g => g.Id == groupId);
            if (group == null)
                return NotFound();
            if (topic == null)
                _context.TopicList.Add(ntopic);
            group.Topics.Add(ntopic);
            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {

                return StatusCode(505);
            }
        }

        private bool GroupExists(int id)
        {
            return _context.GroupList.Any(e => e.Id == id);
        }
    }
}
