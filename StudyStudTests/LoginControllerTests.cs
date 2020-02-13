using NUnit.Framework;
using StudyStud.Models;
using NSubstitute;
using NUnit;
using Microsoft.AspNetCore.Identity;
using StudyStud.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace StudyStudTests
{   
    [TestFixture]
    class LoginControllerTests
    {
        SignInManager<User> _signInManager = Substitute.For<FakeSignInManager>();
        Microsoft.AspNetCore.Identity.SignInResult result = Substitute.For<Microsoft.AspNetCore.Identity.SignInResult>();
        LoginController loginController;
        

        [Test]
        public void TestLoginIsSuccessful()
        {
            loginController = new LoginController(_signInManager, Substitute.For<StudyDbContext>(new DbContextOptions<StudyDbContext>()));
            result = Microsoft.AspNetCore.Identity.SignInResult.Success;
            _signInManager.PasswordSignInAsync("Username", "Password69!", false, false).Returns(result);

            Login login = new Login();
            login.Username = "Username";
            login.Password = "Password69!";
            var expected = new OkResult();
            var actual = loginController.Login(login).Result;
            System.Console.WriteLine(expected);

            Assert.That(expected.GetType(), Is.EqualTo(actual.GetType()));
        }
    }
}
