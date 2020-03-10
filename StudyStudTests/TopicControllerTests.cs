﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using StudyStud.Models;
using Microsoft.EntityFrameworkCore;
using StudyStud.Controllers;
using Microsoft.AspNetCore.Mvc;
using StudyStudTests.Utilities;

namespace StudyStudTests
{
    [TestFixture]
    class TopicControllerTests
    {

        List<Topic> _topics;
        StudyDbContext _context = new StudyDbContext(new DbContextOptionsBuilder<StudyDbContext>()
            .UseInMemoryDatabase(databaseName: "users")
            .Options);
        TopicController _topicController;
        User _user;
        

        [SetUp]
        public void SetUp()
        {
            var topic1 = new Topic();
            var topic2 = new Topic { Description = "asdasd", Id = 4, OwnerId = "ad", OwnerName = "Sanyi", Title = "Sanyitopic" };
            var topic3 = new Topic { Description = "aaaaaaa", Id = 3, OwnerId = "ad", OwnerName = "Sanyi", Posts = new List<Post> { new Post(), new Post { Id = 5, OwnerId = "si", OwnerName = "Kata", Message = "asda", TopicID = 3 } } };
            _user = new User { Id = "ad", NickName = "Sanya", UserName = "Sanyi" };
            _context.UserList.Add(_user);
            _topics = new List<Topic> { topic1, topic2, topic3 };
            _topics.ForEach(t => _context.TopicList.Add(t));
            _context.SaveChanges();
            _topicController = new TopicController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.UserList.Remove(_user);
            _context.TopicList.RemoveRange(_topics);
            _context.SaveChanges();
            _topics = null;
        } 

        [Test]
        public void GetTopics_ReturnSuccess()
        {
            _topics.Sort(new TopicComparer());
            _topics.Reverse();
            var expected = new OkObjectResult(_topics).Value;
            var actual = ((OkObjectResult)_topicController.GetTopics().Result).Value;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddTopic_GivenValidTopic_ReturnSuccess()
        {
            var newTopic = new Topic { OwnerId = "ad" };
            _topics.Add(newTopic);
            var expected = new OkResult().StatusCode;
            var actual = ((OkResult)_topicController.AddTopic(newTopic).Result).StatusCode;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddTopic_GivenTopicWithoutOwnerId_ReturnStatusCode406()
        {
            var newTopic = new Topic();

            var expected = new StatusCodeResult(406).StatusCode;
            var actual = ((StatusCodeResult)_topicController.AddTopic(newTopic).Result).StatusCode;
            
            Assert.AreEqual(expected, actual);
        }
    }
}
