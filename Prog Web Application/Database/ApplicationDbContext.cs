using System;
using Microsoft.EntityFrameworkCore;
using Prog_Web_Application.Models;

namespace Prog_Web_Application.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Define a DbSet for the User model
        public DbSet<User> Users { get; set; }
        public DbSet<Claim> Claims { get; set; }

        // Optional: Override OnModelCreating for additional configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the User entity
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id); // Set Id as the primary key

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired() // Make Username required
                .HasMaxLength(50); // Set max length for Username

            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired() // Make Password required
                .HasMaxLength(100); // Set max length for Password

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .IsRequired() // Make Role required
                .HasMaxLength(50); // Set max length for Role
// -------------------------------------------------------------------------------------------------
            // Update Claim configuration
            modelBuilder.Entity<Claim>()
                .HasKey(c => c.ClaimID); // Set ClaimID as the primary key

            modelBuilder.Entity<Claim>()
                .Property(c => c.FullName)
                .IsRequired() // Make FullName required
                .HasMaxLength(100); // Set max length for FullName

            modelBuilder.Entity<Claim>()
                .Property(c => c.Username)
                .IsRequired() // Make Username required
                .HasMaxLength(50); // Set max length for Username

            modelBuilder.Entity<Claim>()
                .Property(c => c.Email)
                .IsRequired() // Make Email required
                .HasMaxLength(100); // Set max length for Email

            modelBuilder.Entity<Claim>()
                .Property(c => c.Phone)
                .IsRequired() // Make Phone required
                .HasMaxLength(15); // Set max length for Phone

            modelBuilder.Entity<Claim>()
                .Property(c => c.ClaimDescription)
                .IsRequired() // Make ClaimDescription required
                .HasMaxLength(200); // Set max length for ClaimDescription

            modelBuilder.Entity<Claim>()
                .Property(c => c.uploadedFile); 
                
            modelBuilder.Entity<Claim>()
                .Property(c => c.HoursWorked)
                .IsRequired(); // Make HoursWorked required

            modelBuilder.Entity<Claim>()
                .Property(c => c.HourlyRate)
                .IsRequired(); // Make HourlyRate required

            modelBuilder.Entity<Claim>()
                .Property(c => c.submissionDate)
                .IsRequired(); // Make submissionDate required

            modelBuilder.Entity<Claim>()
                .Property(c => c.Status)
                .HasConversion<string>() 
                .IsRequired(); // Set as required
        }
    }
}

            