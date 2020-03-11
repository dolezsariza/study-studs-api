using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyStud.Models
{
    public class StudyDbContext : IdentityDbContext<User>
    {
        public StudyDbContext(DbContextOptions<StudyDbContext> options):base(options){
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GroupUser>()
                .HasKey(bc => new { bc.GroupId, bc.UserId });
            modelBuilder.Entity<GroupUser>()
                .HasOne(bc => bc.Group)
                .WithMany(b => b.GroupUsers)
                .HasForeignKey(bc => bc.GroupId);
            modelBuilder.Entity<GroupUser>()
                .HasOne(bc => bc.User)
                .WithMany(c => c.GroupUsers)
                .HasForeignKey(bc => bc.UserId);

            //modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        }

        public DbSet<User> UserList { get; set; }
        public DbSet<Topic> TopicList { get; set; }
        public DbSet<Post> PostList { get; set; }
        public DbSet<Group> GroupList { get; set; }
    }
}
