using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NSubstitute;
using NUnit.Framework;
using StudyStud.Controllers;
using StudyStud.Models;

namespace StudyStudTests
{
    [TestFixture]
    public class RegisterControllerTests
    {
        private UserManager<User> _userManager;
        private StudyDbContext _studyDbContext;
        private RegisterController _registerController;
        private User _user;
        private const string GOOD_JSON = "{\"email\" : \"username@username.com\", \"userName\" : \"Username\", \"password\" : \"Password69!\"}";
        private const string BAD_JSON = "{\"email\" : \"username@username.com\", \"userName\" : \"Username\", \"password\" : \"password\"}";

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
            _user.Password = "Password69!";
            _userManager.CreateAsync(Arg.Any<User>(),"Password69!").Returns(registerResult);

            var expected = new CreatedResult("", null);
            var actual = _registerController.Register(JObject.Parse(GOOD_JSON)).Result;

            Assert.AreEqual(expected.GetType(), actual.GetType());
        }

        [Test]
        public void TestRegisterGivenWrongCredentialsReturnFailed()
        {
            var registerResult = IdentityResult.Failed();
            _userManager.CreateAsync(Arg.Any<User>(), "password").Returns(registerResult);

            var expected = new ObjectResult(new StatusCodeResult(406));
            var actual = _registerController.Register(JObject.Parse(BAD_JSON)).Result;

            Assert.AreEqual(expected.GetType(), actual.GetType());
        }
    }
}