using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Prog_Web_Application.Models;

namespace Prog_Web_Application.Database
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Claim> Claims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.FullName)
                    .HasMaxLength(100)
                    .IsRequired(false);

                entity.Property(u => u.Role)
                    .HasMaxLength(50)
                    .IsRequired(false);
            });

            // Claim Configuration
            modelBuilder.Entity<Claim>(entity =>
            {
                entity.HasKey(c => c.ClaimID);

                entity.Property(c => c.FullName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                entity.Property(c => c.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                entity.Property(c => c.Phone)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnType("nvarchar(15)");

                entity.Property(c => c.ClaimDescription)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar(200)");

                entity.Property(c => c.HoursWorked)
                    .IsRequired()
                    .HasColumnType("int");

                entity.Property(c => c.HourlyRate)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(c => c.SubmissionDate)
                    .IsRequired()
                    .HasColumnType("datetime2");

                entity.Property(c => c.Status)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .HasColumnType("nvarchar(20)");

                entity.Property(c => c.FileName)
                    .IsRequired(false)
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                entity.Property(c => c.AdditionalNotes)
                    .IsRequired(false)
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar(200)");

                entity.Property(c => c.UploadedFile)
                    .IsRequired(false)
                    .HasColumnType("BLOB"); // Use BLOB for SQLite
            });
        }
    }
}