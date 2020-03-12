using NUnit.Framework;
using StudyStud.Models;
using NSubstitute;
using Microsoft.AspNetCore.Identity;
using StudyStud.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace StudyStudTests
{   
    [TestFixture]
    class LoginControllerTests
    {
        UserManager<User> _userManager;
        StudyDbContext _studyDbContext;
        SignInManager<User> _signInManager;
        LoginController _loginController;
        Login _login;
        
        [SetUp]
        public void Setup()
        {
            _userManager = Substitute.For<FakeUserManager>();
            _studyDbContext = Substitute.For<StudyDbContext>(new DbContextOptions<StudyDbContext>());
            //_loginController = new LoginController(_studyDbContext, _userManager);
            _login = new Login();
            _login.Username = "TestUser";
            _login.Password = "password";
        }

        [Test]
        public void TestLoginIsSuccessfull()
        {
            User user = new User();
            user.UserName = "TestUser";
            user.Id = "TestId";
            user.Email = "TestEmail";
            Task<bool> result = Task.FromResult(true);
            _userManager.FindByNameAsync(user.UserName).Returns(user);
            _userManager.CheckPasswordAsync(user, "password").Returns(result);
            _signInManager.SignInAsync(user, true).Returns(result);

            var expected = new OkObjectResult(null);
            var actual = (ObjectResult) _loginController.Login(_login).Result;

            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
        }

        [Test]
        public void TestLoginIsFailed() 
        {
            User user = new User();
            Task<bool> result = Task.FromResult(false);
            _userManager.CheckPasswordAsync(user, "password").Returns(result);

            var expected = new BadRequestObjectResult(new BadRequestResult());
            var actual = _loginController.Login(_login).Result;

            Assert.AreEqual(expected.GetType(), actual.GetType());
        }
    }
}
