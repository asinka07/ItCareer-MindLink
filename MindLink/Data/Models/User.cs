namespace MindLink.Data.Models
{
    public class User
    {
        // PK
        public string UserCode { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;

        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public DateTime LastLogin { get; set; }
        public char Gender { get; set; }

        public DateTime Birthday { get; set; }

        public ICollection<Record> Records { get; set; } = new List<Record>();
    }
}
