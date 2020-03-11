using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace StudyStud.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string Introduction { get; set; }
        public string Interests { get; set; }
        public string School { get; set; }
        public string City { get; set; }
        public string ProfilePicture { get; set; }
        public List<Group> Groups { get; set; }
    }
}
