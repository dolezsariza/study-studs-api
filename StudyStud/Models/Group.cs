using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudyStud.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        public List<Topic> Topics { get; set; }
        [ForeignKey(nameof(User))]
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public List<User> Users { get; set; }
        public List<GroupUser> GroupUsers { get; set; }

        public Group()
        {
            Date = DateTime.Now;
        }
    }
}
