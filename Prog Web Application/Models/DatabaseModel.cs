/// <summary>
/// These are the basic getters and setters for User and Claim models.
/// It also includes the ClaimStatus enum.
/// It defines the properties of the User and Claim models.
/// </summary>

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

        
        [EmailAddress(ErrorMessage = "Please enter a valid email address such as JohnDoe@mail.com")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Please enter a valid phone number such as 123-456-7890")]
        public string Phone { get; set; }

        public string ClaimDescription { get; set; }

        public byte[]? uploadedFile { get; set; } // Byte array for Blob storage

        [Range(1, int.MaxValue, ErrorMessage = "Hours worked must be greater than 0")]
        public int HoursWorked { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Hourly rate must be greater than 0")]
        public decimal HourlyRate { get; set; }

        public DateTime submissionDate { get; set; }

        public string? FileName { get; set; } // Store file name with blob

        public ClaimStatus Status { get; set; } = ClaimStatus.Pending; // Default is Pending

        public string? AdditionalNotes { get; set; }
     
    }

    // Claim status enum
    public enum ClaimStatus
    {
        Pending,
        Approved,
        Rejected
    }
}

// --------------------------------------------**End of File**--------------------------------------------------------