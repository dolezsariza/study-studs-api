using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudyStud.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; }
        public Topic Topic { get; }
        public string Title { get; set; }
        public string Message { get; set; }

        public Post()
        {

        }

        public Post(User owner, Topic topic, string title, string message)
        {
            UserId = owner.Id;
            Topic = topic;
            Title = title;
            Message = message;
        }
    }
}
