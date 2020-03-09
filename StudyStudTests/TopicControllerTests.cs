using System;
using System.Collections.Generic;
using NUnit.Framework;
using StudyStud.Models;
using NSubstitute;
using Microsoft.EntityFrameworkCore;
using StudyStud.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace StudyStudTests
{
    [TestFixture]
    class TopicControllerTests
    {

        List<Topic> _topics;
        StudyDbContext _context;
        TopicController _topicController;
        

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<StudyDbContext>()
            .UseInMemoryDatabase(databaseName: "users")
            .Options;
            var topic1 = new Topic { Description = "asdasd", Id = 0, OwnerId = "ad", OwnerName = "Sanyi", Title = "Sanyitopic" };
            var topic2 = new Topic();
            var topic3 = new Topic { Description = "aaaaaaa", Id = 3, OwnerId = "ad", OwnerName = "Sanyi", Posts = new List<Post> { new Post(), new Post { Id = 5, OwnerId = "si", OwnerName = "Kata", Message = "asda", TopicID = 3 } } };
            _topics = new List<Topic> { topic1, topic2, topic3 };
            _context = new StudyDbContext(options);
            _topics.ForEach(t => _context.TopicList.Add(t));
            _context.SaveChanges();
            _topicController = new TopicController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context = null;
        } 

        [Test]
        public void TestGetTopicsReturnSuccess()
        {
            var expected = new OkObjectResult(_topics);
            var actual = _topicController.GetTopics();
            Assert.AreEqual(expected.GetType(), actual.Result.GetType());
        }
    }
}
