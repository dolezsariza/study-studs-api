using Microsoft.AspNetCore.Identity;

namespace StudyStud.Models
{
    public class User : IdentityUser
    {
        public string City { get; set; }
        public string Password { get; set; }
    }
}
