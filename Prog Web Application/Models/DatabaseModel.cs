using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Prog_Web_Application.Models
{
    // User model
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; } 
    }

    // Claim model
    public class Claim
    {
        public int ClaimID { get; set; }
       
        public string FullName { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string ClaimDescription { get; set; }

        public byte[]? uploadedFile { get; set; } // Byte array for Blob storage

        public int HoursWorked { get; set; }

        public decimal HourlyRate { get; set; }

        public DateTime submissionDate { get; set; }

        public string? FileName { get; set; } // Store file name with blob

        public ClaimStatus Status { get; set; } = ClaimStatus.Pending; // Default is Pending
     
    }

    // Claim status enum
    public enum ClaimStatus
    {
        Pending,
        Approved,
        Rejected
    }
}