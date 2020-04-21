using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyStud.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        [ForeignKey(nameof(Topic))]
        public int TopicID { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }

        public Post()
        {
            Date = DateTime.Now;
        }
    }
}
