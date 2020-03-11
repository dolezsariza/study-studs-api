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
        public async Task<IActionResult> ModifyGroup(int id,[FromBody]Group group)
        {
            if (id != group.Id)
            {
                return BadRequest();
            }

            _context.Entry(group).State = EntityState.Modified;

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
        public async Task<ActionResult<Group>> DeleteGroup(int id)
        {
            var group = await _context.GroupList.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            _context.GroupList.Remove(group);
            await _context.SaveChangesAsync();

            return group;
        }

        private bool GroupExists(int id)
        {
            return _context.GroupList.Any(e => e.Id == id);
        }
    }
}
