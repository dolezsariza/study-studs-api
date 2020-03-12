using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using StudyStud.Controllers;
using NSubstitute;
using StudyStud.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace StudyStudTests
{
    [TestFixture]
    class GroupControllerTests
    {
        List<Topic> _topics;
        List<Group> _groups;
        List<GroupUser> _groupUsers;
        StudyDbContext _context = new StudyDbContext(new DbContextOptionsBuilder<StudyDbContext>()
        .UseInMemoryDatabase(databaseName: "groups")
        .Options);
        GroupController _groupController;
        User _user = new User { Id = "ad", NickName = "Sanya", UserName = "Sanyi" };

        [SetUp]
        public void SetUp()
        {
            var topic1 = new Topic();
            var topic2 = new Topic { Description = "asdasd", Id = 4, OwnerId = "ad", OwnerName = "Sanyi", GroupId = 2, Title = "Sanyitopic" };
            var topic3 = new Topic { Description = "aaaaaaa", Id = 3, OwnerId = "ad", OwnerName = "Sanyi", GroupId = 2};
            _context.UserList.Add(_user);
            _topics = new List<Topic> { topic1, topic2, topic3 };
            _topics.ForEach(t => _context.TopicList.Add(t));
            var group1 = new Group { Id = 2, Title = "Title", Description = "Description", OwnerId = "ad" };
            var group2 = new Group();
            _groups = new List<Group> { group1, group2 };
            _context.GroupList.AddRange(_groups);
            _groupUsers = new List<GroupUser> { new GroupUser { Group = group1, GroupId = 2, User = _user, UserId = _user.Id } };
            _context.SaveChanges();
            _groupController = new GroupController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.UserList.Remove(_user);
            _context.TopicList.RemoveRange(_topics);
            _context.GroupList.RemoveRange(_groups);
            _context.GroupUsers.RemoveRange(_groupUsers);
            _groupController = null;
        }

        [Test]
        public void GetAllGroups_ReturnOk()
        {
            var result = new List<Group>();
            result.Add(_groups[0]);
            result[0].Topics.Add(_topics[1]);
            result[0].Topics.Add(_topics[2]);
            result.Add(_groups[1]);
            var expected = new OkObjectResult(result);
            var actual = (ObjectResult)_groupController.GetAllGroups().Result;
            Assert.AreEqual(expected.Value, actual.Value);
        }
    }
}
