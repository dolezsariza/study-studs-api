﻿using Microsoft.EntityFrameworkCore;
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
            modelBuilder.Entity<GroupUser>().HasKey(gu => new {gu.UserId , gu.GroupId });

            //modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        }

        public DbSet<User> UserList { get; set; }
        public DbSet<Topic> TopicList { get; set; }
        public DbSet<Post> PostList { get; set; }
        public DbSet<Group> GroupList { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }

        public DbSet<AppFile> FileList { get; set; }
    }
}
