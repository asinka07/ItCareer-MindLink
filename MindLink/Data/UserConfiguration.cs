using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MindLink.Data.Models;
using static MudBlazor.CategoryTypes;

namespace MindLink.Data
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        private readonly List<User> Special = new List<User>()
        {
            new User
            {
                UserCode = "bLZoUT",
                Username = "admin_user",
                Password = "AQAAAAIAAYagAAAAEJJG/NCL8BXPg/UXNCdW63SHrXqyt4M/Yuf5jkyxzlJhBUdahGYJiAJsc4ioN89azA==",
                Name = "Super Admin",
                RoleId = 2,
                LastLogin = DateTime.Parse("2026-02-14 22:46:55.8797045"),
                Gender = 'm',
                Birthday = DateTime.Parse("2000-10-10 00:00:00.0000000"),
                CreatedAt = DateTime.Parse("2026-02-14 09:43:19.1400000")
            },
            new User
            {
                UserCode = "111111",
                Username = "Error",
                Password = "111111",
                Name = "Error",
                RoleId = 1,
                LastLogin = DateTime.Parse("2026-02-14 22:46:55.8797045"),
                Gender = 'f',
                Birthday = DateTime.Parse("2000-10-10 00:00:00.0000000"),
                CreatedAt = DateTime.Parse("2026-02-14 09:43:19.1400000")
            },
        };

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(Special);
        }
    }
}
