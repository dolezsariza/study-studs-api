using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyStud.Models
{
    public class User : IdentityUser
    {
        public string City { get; set; }
        public string Password { get; set; }
    }
}
