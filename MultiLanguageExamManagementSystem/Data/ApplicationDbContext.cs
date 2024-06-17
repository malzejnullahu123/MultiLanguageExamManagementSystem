﻿using Microsoft.EntityFrameworkCore;
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
        
        public DbSet<User> Users { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<TakenExam> TakenExams { get; set; }
        public DbSet<ExamRequest> ExamRequests { get; set; }

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
            
            
            
            //
            modelBuilder.Entity<ExamQuestion>()
                .HasKey(eq => new { eq.ExamId, eq.QuestionId });

            modelBuilder.Entity<ExamQuestion>()
                .HasOne(eq => eq.Exam)
                .WithMany(e => e.ExamQuestions)
                .HasForeignKey(eq => eq.ExamId);

            modelBuilder.Entity<ExamQuestion>()
                .HasOne(eq => eq.Question)
                .WithMany(q => q.ExamQuestions)
                .HasForeignKey(eq => eq.QuestionId);

            modelBuilder.Entity<TakenExam>()
                .HasOne(te => te.Exam)
                .WithMany(e => e.TakenExams)
                .HasForeignKey(te => te.ExamId);

            modelBuilder.Entity<TakenExam>()
                .HasOne(te => te.Student)
                .WithMany(u => u.TakenExams)
                .HasForeignKey(te => te.StudentId);
        }
    }
}
