
using Microsoft.EntityFrameworkCore;
using WorkshopHubAPI.Models;

namespace WorkshopHubAPI.Data
{
    public class WorkshopHubDbContext : DbContext
    {
        public WorkshopHubDbContext(
            DbContextOptions<WorkshopHubDbContext> options)
            : base(options)
        {
        }

        public DbSet<Trainer> Trainers { get; set; } = null!;
        public DbSet<Workshop> Workshops { get; set; } = null!;
        public DbSet<Trainee> Trainees { get; set; } = null!;
        public DbSet<TraineeProfile> TraineeProfiles { get; set; } = null!;
        public DbSet<Registration> Registrations { get; set; } = null!;
        public DbSet<WorkshopSession> WorkshopSessions { get; set; } = null!;
        public DbSet<Attendance> Attendances { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // One-to-Many: Trainer -> Workshops
            modelBuilder.Entity<Workshop>()
                .HasOne(w => w.Trainer)
                .WithMany(t => t.Workshops)
                .HasForeignKey(w => w.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-One: Trainee -> TraineeProfile
            modelBuilder.Entity<TraineeProfile>()
                .HasOne(p => p.Trainee)
                .WithOne(t => t.TraineeProfile)
                .HasForeignKey<TraineeProfile>(p => p.TraineeId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: Workshop -> Sessions
            modelBuilder.Entity<WorkshopSession>()
                .HasOne(s => s.Workshop)
                .WithMany(w => w.Sessions)
                .HasForeignKey(s => s.WorkshopId)
                .OnDelete(DeleteBehavior.Restrict);

            // Many-to-Many using Registration
            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Trainee)
                .WithMany(t => t.Registrations)
                .HasForeignKey(r => r.TraineeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Workshop)
                .WithMany(w => w.Registrations)
                .HasForeignKey(r => r.WorkshopId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent duplicate registration
            modelBuilder.Entity<Registration>()
                .HasIndex(r => new { r.TraineeId, r.WorkshopId })
                .IsUnique();

            // Registration -> Attendances
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Registration)
                .WithMany(r => r.Attendances)
                .HasForeignKey(a => a.RegistrationId)
                .OnDelete(DeleteBehavior.Restrict);

            // WorkshopSession -> Attendances
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.WorkshopSession)
                .WithMany(s => s.Attendances)
                .HasForeignKey(a => a.WorkshopSessionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent duplicate attendance
            modelBuilder.Entity<Attendance>()
                .HasIndex(a => new
                {
                    a.RegistrationId,
                    a.WorkshopSessionId
                })
                .IsUnique();

            // Fluent API property configuration
            modelBuilder.Entity<Workshop>()
                .Property(w => w.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Trainer>()
                .Property(t => t.FullName)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
