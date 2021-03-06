﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudyStud.Models
{
    public class Topic
    {
        [Key]
        public int Id { get; set; }
        public int? GroupId { get; set; }
        public List<Post> Posts { get; set; }
        [ForeignKey(nameof(User))]
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public List<AppFile> Files { get; set; }

        public Topic()
        {
            Date = DateTime.Now;
        }
    }
}
