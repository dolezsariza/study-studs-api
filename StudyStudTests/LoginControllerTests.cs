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
        SignInManager<User> _signInManager;
        StudyDbContext _studyDbContext;
        SignInResult signInResult;
        LoginController _loginController;
        Login _login;
        
        [SetUp]
        public void Setup()
        {
            _signInManager = Substitute.For<FakeSignInManager>();
            _studyDbContext = Substitute.For<StudyDbContext>(new DbContextOptions<StudyDbContext>());
            _loginController = new LoginController(_signInManager, _studyDbContext);
            _login = new Login();
            _login.Username = "Username";
            _login.Password = "Password69!";
        }

        [Test]
        public void TestLoginIsSuccessful()
        {
            signInResult = SignInResult.Success;
            _signInManager.PasswordSignInAsync("Username", "Password69!", false, false).Returns(signInResult);

            var expected = new OkResult();
            var actual = _loginController.Login(_login).Result;

            Assert.AreEqual(expected.GetType(), actual.GetType());
        }

        [Test]
        public void TestLoginIsFailed()
        {
            signInResult = SignInResult.Failed;
            _signInManager.PasswordSignInAsync("Username", "Password69!", false, false).Returns(signInResult);

            var expected = new UnauthorizedObjectResult(new UnauthorizedResult());
            var actual = _loginController.Login(_login).Result;

            Assert.AreEqual(expected.GetType(), actual.GetType());
        }
    }
}
