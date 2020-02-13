using Microsoft.AspNetCore.Identity;
using StudyStud.Models;
using System;
using NSubstitute;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace StudyStudTests
{
    class FakeUserManager : UserManager<User>
    {
        public FakeUserManager()
            : base(Substitute.For<IUserStore<User>>(),
                  Substitute.For<IOptions<IdentityOptions>>(),
                  Substitute.For<IPasswordHasher<User>>(),
                  new IUserValidator<User>[0],
                  new IPasswordValidator<User>[0],
                  Substitute.For<ILookupNormalizer>(),
                  Substitute.For<IdentityErrorDescriber>(),
                  Substitute.For<IServiceProvider>(),
                  Substitute.For<ILogger<UserManager<User>>>())
        { }
    }
}
