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
        public User Owner { get; set; }
        public Topic Topic { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public Post()
        {

        }

        public Post(User owner, Topic topic, string title, string message)
        {
            Owner = owner;
            Topic = topic;
            Title = title;
            Message = message;
        }
    }
}
