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

            modelBuilder.Entity<Language>()
                .HasMany<LocalizationResource>()
                .WithOne(lr => lr.Language)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Language>()
                .HasIndex(l => new {l.CountryId, l.LanguageCode})
                .IsUnique();
            
            modelBuilder.Entity<Country>()
                .HasMany<Language>()
                .WithOne(l => l.Country)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<LocalizationResource>()
                .HasIndex(lr => new { lr.LanguageId, lr.Namespace, lr.Key })
                .IsUnique();
        }
    }
}