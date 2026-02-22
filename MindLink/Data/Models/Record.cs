namespace MindLink.Data.Models
{
    public class Record
    {
        public int Id { get; set; }

        public string UserCode { get; set; } = null!;
        public User User { get; set; } = null!;

        public string RecordText { get; set; } = null!;
        public DateTime RecordDate { get; set; }
        public string? Sentiment { get; set; }
        public int Rate { get; set; }
        public string? Sentiment { get; set; } // for AI

    }
}
