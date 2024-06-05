using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Models.Entities;

namespace MultiLanguageExamManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }

        public DbSet<Language> Languages { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<LocalizationResource> LocalizationResources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // One country can have multiple languages
            modelBuilder.Entity<Country>()
                .HasMany(c => c.Languages)
                .WithOne(l => l.Country)
                .OnDelete(DeleteBehavior.Cascade);

            // One language can have multiple localization resources
            modelBuilder.Entity<Language>()
                .HasMany(l => l.LocalizationResources)
                .WithOne(lr => lr.Language)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique index on namespace and key in LocalizationResource
            modelBuilder.Entity<LocalizationResource>()
                .HasIndex(lr => new { lr.Namespace, lr.Key })
                .IsUnique();
        }
    }
}