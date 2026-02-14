using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MindLink.Data.Models
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public string UserCode { get; set; } = null!;

        [ForeignKey(nameof(UserCode))]
        public User User { get; set; } = null!;

        public DateTime Date { get; set; }

        public string Status { get; set; }
    }
}
