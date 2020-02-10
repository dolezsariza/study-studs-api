using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyStud.Models
{
    public class StudyDbContext : DbContext
    {
        protected StudyDbContext(DbContextOptions<StudyDbContext> options):base(options){}
    }
}
