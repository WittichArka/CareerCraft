using CareerCraft.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareerCraft.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
public DbSet<Skill> Skills { get; set; }
public DbSet<User> Users { get; set; }
public DbSet<UserInfo> UserInfos { get; set; }
public DbSet<Vacancy> Vacancies { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Vacancy>(entity =>
    {
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => new { e.SourceName, e.ExternalId }).IsUnique();
        entity.Property(e => e.SourceName).IsRequired().HasMaxLength(100);
        entity.Property(e => e.ExternalId).IsRequired().HasMaxLength(100);
    });


        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);

            entity.HasMany(e => e.UserInfos)
                  .WithOne(e => e.User)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Value).IsRequired();
            entity.Property(e => e.Order).IsRequired();
        });
    }
}
