using System.ComponentModel.DataAnnotations;

namespace MindLink.Data.Models
{
    public class Resource
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Emotion { get; set; }
        public bool IsVisible { get; set; } = true;
        public DateTime CreatedOn { get; set; }
    }
}
