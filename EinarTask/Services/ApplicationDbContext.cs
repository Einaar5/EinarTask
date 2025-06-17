using EinarTask.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EinarTask.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TaskType> TaskTypes { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // TaskType yapılandırması
            builder.Entity<TaskType>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(100);
                entity.Property(e => e.Color).HasMaxLength(7);

                // User ile ilişki
                entity.HasOne(e => e.User)
                      .WithMany(u => u.TaskTypes)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.NoAction); // Cascade çakışmasını önlemek için

                // Index tanımlamaları
                entity.HasIndex(e => new { e.UserId, e.Name }).IsUnique();
            });

            // Task yapılandırması
            builder.Entity<Models.Task>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);

                // User ile ilişki
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Tasks)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.NoAction); // Cascade çakışmasını önlemek için

                // TaskType ile ilişki
                entity.HasOne(e => e.TaskType)
                      .WithMany(tt => tt.Tasks)
                      .HasForeignKey(e => e.TaskTypeId)
                      .OnDelete(DeleteBehavior.NoAction); // Cascade çakışmasını önlemek için
            });

            // User yapılandırması
            builder.Entity<User>(entity =>
            {
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            });
        }
    }
}