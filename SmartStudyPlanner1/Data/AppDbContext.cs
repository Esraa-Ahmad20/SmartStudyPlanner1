using Microsoft.EntityFrameworkCore;
using SmartStudyPlanner1.Models;

namespace SmartStudyPlanner1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<StudyPlan> StudyPlans { get; set; }
        public DbSet<StudyTask> StudyTasks { get; set; }
        public DbSet<ProgressRecord> ProgressRecords { get; set; }
        public DbSet<MotivationQuote> MotivationQuotes { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<PomodoroSession> PomodoroSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProgressRecord>()
                .HasOne(p => p.StudyTask)
                .WithOne(t => t.ProgressRecord)
                .HasForeignKey<ProgressRecord>(p => p.TaskId);

            modelBuilder.Entity<ProgressRecord>()
                .HasIndex(p => p.TaskId).IsUnique();

            modelBuilder.Entity<StudyTask>()
                .HasOne(t => t.StudyPlan)
                .WithMany(p => p.StudyTasks)
                .HasForeignKey(t => t.PlanId);

            modelBuilder.Entity<StudyTask>()
                .HasOne(t => t.Chapter)
                .WithMany(c => c.StudyTasks)
                .HasForeignKey(t => t.ChapterId);

            modelBuilder.Entity<Subject>()
                .HasOne(s => s.User)
                .WithMany(u => u.Subjects)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<StudyPlan>()
                .HasOne(p => p.User)
                .WithMany(u => u.StudyPlans)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Chapter>()
                .HasOne(c => c.Subject)
                .WithMany(s => s.Chapters)
                .HasForeignKey(c => c.SubjectId);

            modelBuilder.Entity<Resource>()
                .HasOne(r => r.Subject)
                .WithMany(s => s.Resources)
                .HasForeignKey(r => r.SubjectId);

            modelBuilder.Entity<PomodoroSession>()
                .HasOne(p => p.Subject)
                .WithMany(s => s.PomodoroSessions)
                .HasForeignKey(p => p.SubjectId);

            modelBuilder.Entity<StudyTask>()
                .Property(t => t.Priority).HasDefaultValue(1);

            base.OnModelCreating(modelBuilder);
        }
    }
}