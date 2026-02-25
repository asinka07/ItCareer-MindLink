using Microsoft.EntityFrameworkCore;
using MindLink.Data.Models;
using System.Reflection.Emit;

namespace MindLink.Data;

public class MindLinkDbContext : DbContext
{
    public MindLinkDbContext(DbContextOptions<MindLinkDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Record> Records => Set<Record>();
    public DbSet<Log> Log => Set<Log>();
    public DbSet<Resource> Resources => Set<Resource>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Roles
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name)
                  .HasColumnName("Role")
                  .HasMaxLength(50)
                  .IsRequired();
        });

        // Users
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.UserCode);

            entity.Property(u => u.UserCode)
                  .HasColumnType("char(6)");

            entity.Property(u => u.Username)
                  .HasMaxLength(50)
                  .IsRequired();

            entity.Property(u => u.Password)
                  .HasMaxLength(1000)
                  .IsRequired();

            entity.Property(u => u.Name)
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(u => u.Gender)
                  .HasColumnType("char(1)");

            entity.Property(u => u.CreatedAt)
                  .HasColumnType("datetime2")
                  .HasDefaultValueSql("GETDATE()")
                  .IsRequired();

            entity.Property(u => u.LastLogin)
                  .HasColumnType("datetime2");

            entity.HasOne(u => u.Role)
                  .WithMany(r => r.Users)
                  .HasForeignKey(u => u.RoleId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.Role)
                  .WithMany(r => r.Users)
                  .HasForeignKey(u => u.RoleId)
                  .OnDelete(DeleteBehavior.Restrict);

        });

        modelBuilder.Entity<Record>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.UserCode)
                  .HasColumnType("char(6)")
                  .IsRequired();

            entity.Property(r => r.RecordText)
                  .HasColumnType("text")
                  .IsRequired();

            entity.HasOne(r => r.User)
                  .WithMany(u => u.Records)
                  .HasForeignKey(r => r.UserCode)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.Property(r => r.Sentiment)
            .HasColumnType("nvarchar(20)");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.Title)
                  .HasColumnType("nvachar(150)")
                  .IsRequired();

            entity.Property(r => r.Content)
                  .HasColumnType("text")
                  .IsRequired();

            entity.Property(r => r.Emotion)
                  .HasColumnType("nvarchar(10)")
                  .IsRequired();

            entity.Property(r => r.IsVisible)
                  .HasColumnType("bit")
                  .IsRequired();

            entity.Property(u => u.CreatedOn)
                  .HasColumnType("datetime2")
                  .HasDefaultValueSql("GETDATE()")
                  .IsRequired();
        });

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MindLinkDbContext).Assembly);
    }
}
