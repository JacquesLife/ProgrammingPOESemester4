using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Prog_Web_Application.Models
{
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

    public class Claim
    {
        public int ClaimID { get; set; }
       
        public string FullName { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string ClaimDescription { get; set; }

         public byte[]? uploadedFile { get; set; } // Use nullable byte array

        public int HoursWorked { get; set; }

        public decimal HourlyRate { get; set; }

        public DateTime submissionDate { get; set; }

        public ClaimStatus Status { get; set; } = ClaimStatus.Pending; // Default to Pending
     
    }

    public enum ClaimStatus
    {
        Pending,
        Approved,
        Rejected
    }
}