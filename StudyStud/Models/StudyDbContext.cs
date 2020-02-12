using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyStud.Models
{
    public class StudyDbContext : IdentityDbContext<User>
    {
        public StudyDbContext(DbContextOptions<StudyDbContext> options):base(options){}

    }
}
