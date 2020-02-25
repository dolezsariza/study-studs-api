using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudyStud.Models
{
    public class Topic
    {
        [Key]
        public int Id { get; set; }
        public List<Post> Posts { get; }
        public User Owner { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public Topic()
        {

        }

        public Topic(User owner, string title, string description)
        {
            Owner = owner;
            Posts = new List<Post>();
            Title = title;
            Description = description;
        }

        public void AddPost(Post post)
        {
            Posts.Add(post);
        }

        public void RemovePost(Post post)
        {
            Posts.Remove(post);
        }
    }
}
