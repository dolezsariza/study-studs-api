using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NSubstitute;
using NUnit.Framework;
using StudyStud.Controllers;
using StudyStud.Models;
using StudyStud.RequestModels;

namespace StudyStudTests
{
    [TestFixture]
    public class RegisterControllerTests
    {
        private UserManager<User> _userManager;
        private StudyDbContext _studyDbContext;
        private RegisterController _registerController;
        private User _user;
        private readonly RegisterPostRequest _good = new RegisterPostRequest { Email = "username@username.com", Username = "Username", Password = "Password69!"};
        private readonly RegisterPostRequest _bad = new RegisterPostRequest { Email = "username@username.com", Username = "Username", Password = "p" };

        [SetUp]
        public void Setup()
        {
            _userManager = Substitute.For<FakeUserManager>();
            _studyDbContext = Substitute.For<StudyDbContext>(new DbContextOptions<StudyDbContext>());
            _registerController = new RegisterController(_userManager, _studyDbContext);
            _user = new User();
            _user.UserName = "Username";
            _user.Email = "username@username.com";
        }

        [Test]
        public void TestRegisterGivenValidCredentialsReturnCreated()
        {
            var registerResult = IdentityResult.Success;
            _userManager.CreateAsync(Arg.Any<User>(),"Password69!").Returns(registerResult);

            var expected = new CreatedResult("", null);
            var actual = _registerController.Register(_good).Result;

            Assert.AreEqual(expected.GetType(), actual.GetType());
        }

        [Test]
        public void TestRegisterGivenWrongCredentialsReturnFailed()
        {
            var registerResult = IdentityResult.Failed();
            _userManager.CreateAsync(Arg.Any<User>(), "p").Returns(registerResult);

            var expected = new ObjectResult(new StatusCodeResult(406));
            //var actual = _registerController.Register(JObject.Parse(BAD_JSON)).Result;

           // Assert.AreEqual(expected.GetType(), actual.GetType());
        }
    }
}