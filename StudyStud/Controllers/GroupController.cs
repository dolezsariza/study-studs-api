using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyStud.Models;
using StudyStud.RequestModels;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
            groups.ForEach(g => g.Topics = _context.TopicList.Where(t => t.GroupId == g.Id).ToList());
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
            group.Topics = _context.TopicList.Where(t => t.GroupId == id).ToList();
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

            var identified = await UserOwnsGroup(id);
            if (!identified)
            {
                return Unauthorized("You do not have rights to do this");
            }

            group.Title = modifiedGroup.Title;
            group.Description = modifiedGroup.Description;

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

        [HttpPost]
        public async Task<ActionResult<Group>> AddGroup([FromBody] GroupPostRequest groupInfo)
        {
            var group = new Group();
            group.OwnerName = GetUserName();
            group.OwnerId = GetUserId();
            group.Title = groupInfo.Title;
            group.Description = groupInfo.Description;
            var user = await _context.UserList.SingleOrDefaultAsync(u => u.UserName == group.OwnerName);
            if(user == null)
            {
                return NotFound("Wrong request body");
            }
            GroupUser groupUser = new GroupUser { Group = group, GroupId = group.Id, User = user, UserId = user.Id };
            _context.GroupList.Add(group);
            _context.GroupUsers.Add(groupUser);
            await _context.SaveChangesAsync();
            return Created("New group created", "");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGroup(int id)
        {
            var group = await _context.GroupList.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            var identified = await UserOwnsGroup(id);
            if (!identified) 
            {
                return Unauthorized("You do not have rights to do this");
            }
            _context.GroupList.Remove(group);
            _context.GroupUsers.RemoveRange(_context.GroupUsers.Where(g => g.GroupId == id));
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{groupId}/join")]
        public async Task<ActionResult> JoinToGroup(int groupId)
        {
            try
            {
                var user = await _context.UserList.SingleOrDefaultAsync(u => u.UserName == GetUserName());
                var group = await _context.GroupList.SingleOrDefaultAsync(g => g.Id == groupId);
                if (user == null || group == null)
                {
                    return NotFound();
                }
                GroupUser groupUser = new GroupUser { GroupId = group.Id, Group = group, User = user, UserId = user.Id };
                _context.GroupUsers.Add(groupUser);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {

                return StatusCode(505);
            }
        }

        [HttpPost("{groupId}")]
        public async Task<ActionResult> AddTopicToGroup(int groupId, [FromBody]Topic ntopic)
        {
            var topic = await _context.TopicList.SingleOrDefaultAsync(t => t.Id == ntopic.Id);
            var group = await _context.GroupList.SingleOrDefaultAsync(g => g.Id == groupId);
            if (group == null)
                return NotFound();
            if (topic == null)
            {
                ntopic.GroupId = groupId;
                _context.TopicList.Add(ntopic);
            }
            else
                topic.GroupId = groupId;
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

        async private Task<bool> UserOwnsGroup(int groupId)
        {
            var group = await _context.GroupList.SingleOrDefaultAsync(g => g.Id == groupId);
            var userId = group.OwnerId;
            var currentUserId = GetUserId();
            return (currentUserId == userId);
        }


        private string GetUserId()
        {
            return User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
        }
        private string GetUserName()
        {
            return User.Identity.Name;
        }


    }
}
