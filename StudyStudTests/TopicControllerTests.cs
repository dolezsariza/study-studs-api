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
        List<Post> _posts;
        StudyDbContext _context = new StudyDbContext(new DbContextOptionsBuilder<StudyDbContext>()
            .UseInMemoryDatabase(databaseName: "users")
            .Options);
        TopicController _topicController;
        User _user = new User { Id = "ad", NickName = "Sanya", UserName = "Sanyi" };


        [SetUp]
        public void SetUp()
        {
            _posts = new List<Post> { new Post(), new Post { Id = 5, OwnerId = "si", OwnerName = "Kata", Message = "asda", TopicID = 3 } };
            var topic1 = new Topic();
            var topic2 = new Topic { Description = "asdasd", Id = 4, OwnerId = "ad", OwnerName = "Sanyi", Title = "Sanyitopic" };
            var topic3 = new Topic { Description = "aaaaaaa", Id = 3, OwnerId = "ad", OwnerName = "Sanyi", Posts = _posts };
            _context.UserList.Add(_user);
            _topics = new List<Topic> { topic1, topic2, topic3 };
            _topics.ForEach(t => _context.TopicList.Add(t));
            _posts.ForEach(p => _context.PostList.Add(p));
            _context.SaveChanges();
            _topicController = new TopicController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.UserList.Remove(_user);
            _context.TopicList.RemoveRange(_topics);
            _context.PostList.RemoveRange(_posts);
            _context.SaveChanges();
            _topics = null;
            _posts = null;
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
        /*
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
        */
        [Test]
        public void GetTopic_GivenValidId_ReturnOkResult()
        {
            var topicId = 3;
            
            var expected = new OkObjectResult(_topics.Find(t => t.Id == topicId)).Value;
            var actual = ((OkObjectResult)_topicController.GetTopic(topicId).Result).Value;
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetTopic_GivenWrongId_ReturnStatus204()
        {
            var topicId = -1;
            
            var expected = new StatusCodeResult(204);
            var actual = (StatusCodeResult)_topicController.GetTopic(topicId).Result;
            
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
        }

        /*[Test]
        public void AddPostToTopic_GivenValidIdAndPost_ReturnOk()
        {
            var topicId = _topics[1].Id;
            var post = new Post { OwnerId = _user.Id, Message = "Something" };
            
            var expected = new OkResult();
            var actual = (StatusCodeResult)_topicController.AddPostToTopic(topicId, post).Result;
            
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
        }
        */

        [Test]
        public void AddPostToTopic_GivenWrongId_ReturnStatusCode406()
        {
            var topicId = -1;
            var post = new Post { OwnerId = _user.Id, Message = "Something" };
            
            var expected = new StatusCodeResult(406);
            var actual = (StatusCodeResult)_topicController.AddPostToTopic(topicId, post).Result;
            
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
        }

        [Test]
        public void AddPostToTopic_GivenInvalidPost_ReturnStatusCode406()
        {
            var topicId = _topics[1].Id;
            var post = new Post { OwnerId = null };
            
            var expected = new StatusCodeResult(406);
            var actual = (StatusCodeResult)_topicController.AddPostToTopic(topicId, post).Result;
            
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
        }

        [Test]
        public void DeleteTopic_GivenValidTopicIdAndUser_ReturnOk()
        {
            var topicId = 3;
            var user = "{ \"UserId\" : \"ad\"}";
            var expected = new OkResult();
            var actual = (StatusCodeResult)_topicController.DeleteTopic(topicId, user).Result;
            _topics.RemoveAll(t => t.Id == topicId);
            _posts.RemoveAll(p => p.TopicID == topicId);
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
        }

        [Test]
        public void DeleteTopic_GivenInValidTopicId_ReturnStatusCode406()
        {
            var topicId = -1;
            var user = "{ \"UserId\" : \"ad\"}";
            var expected = new StatusCodeResult(406);
            var actual = (StatusCodeResult)_topicController.DeleteTopic(topicId, user).Result;
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
        }

        [Test]
        public void DeleteTopic_GivenInValidUser_ReturnStatusCode406()
        {
            var topicId = 3;
            var user = "";
            var expected = new StatusCodeResult(406);
            var actual = (StatusCodeResult)_topicController.DeleteTopic(topicId, user).Result;
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
        }

        [Test]
        public void DeleteTopic_GivenWrongUser_ReturnUnauthorized()
        {
            var topicId = 3;
            var user = "{ \"UserId\" : \"sat\"}";
            var expected = new UnauthorizedResult();
            var actual = (StatusCodeResult)_topicController.DeleteTopic(topicId, user).Result;
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
        }

        [Test]
        public void DeletePost_GivenValidInputs_ReturnOk()
        {
            var postId = 5;
            var user = "{ \"UserId\" : \"si\"}";
            var expected = new OkResult();
            var actual = (StatusCodeResult)_topicController.DeletePost(postId, user).Result;
            _posts.RemoveAll(p => p.Id == postId);
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
        }

        [Test]
        public void DeletePost_GivenInValidTopicId_ReturnStatusCode406()
        {
            var postId = -1;
            var user = "{ \"UserId\" : \"si\"}";
            var expected = new StatusCodeResult(406);
            var actual = (StatusCodeResult)_topicController.DeletePost(postId, user).Result;
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
        }

        [Test]
        public void DeletePost_GivenInValidUser_ReturnStatusCode406()
        {
            var postId = 5;
            var user = "";
            var expected = new StatusCodeResult(406);
            var actual = (StatusCodeResult)_topicController.DeletePost(postId, user).Result;
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
        }

        [Test]
        public void DeletePost_GivenWrongUser_ReturnUnauthorized()
        {
            var postId = 5;
            var user = "{ \"UserId\" : \"sat\"}";
            var expected = new UnauthorizedResult();
            var actual = (StatusCodeResult)_topicController.DeletePost(postId, user).Result;
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
        }
    }
}
