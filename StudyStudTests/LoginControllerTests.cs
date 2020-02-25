using NUnit.Framework;
using StudyStud.Models;
using NSubstitute;
using Microsoft.AspNetCore.Identity;
using StudyStud.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace StudyStudTests
{   
    [TestFixture]
    class LoginControllerTests
    {
        UserManager<User> _userManager;
        StudyDbContext _studyDbContext;
        SignInResult signInResult;
        LoginController _loginController;
        Login _login;
        
        [SetUp]
        public void Setup()
        {
            _userManager = Substitute.For<FakeUserManager>();
            _studyDbContext = Substitute.For<StudyDbContext>(new DbContextOptions<StudyDbContext>());
            _loginController = new LoginController(_studyDbContext, _userManager);
            _login = new Login();
            _login.Username = "Username";
            _login.Password = "Password69!";
        }

        [Test]
        public void TestLoginIsSuccessful()
        {
            //todo
            var expected = new OkResult();
            var actual = _loginController.Login(_login).Result;

            Assert.AreEqual(expected.GetType(), actual.GetType());
        }

        [Test]
        public void TestLoginIsFailed()
        {
            signInResult = SignInResult.Failed;
           // _signInManager.PasswordSignInAsync("Username", "Password69!", false, false).Returns(signInResult);

            var expected = new UnauthorizedObjectResult(new UnauthorizedResult());
            var actual = _loginController.Login(_login).Result;

            Assert.AreEqual(expected.GetType(), actual.GetType());
        }
    }
}
