using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MindLink.Data.Models;
namespace MindLink.Data
{
    public class RolesConfiguration : IEntityTypeConfiguration<Role>
    {
        private readonly List<Role> Roles = new List<Role>
        {
            new Role
            {
                Id = 1,
                Name = "User"
            },
            new Role
            {
                Id = 2,
                Name = "Admin"
            }
        };
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(Roles);
        }
    }
}
