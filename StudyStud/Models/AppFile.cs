using System.ComponentModel.DataAnnotations.Schema;

namespace StudyStud.Models
{
    public class AppFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        [ForeignKey(nameof(User))]
        public string OwnerId { get; set; }
        public int TopicId { get; set; }
    }
}
