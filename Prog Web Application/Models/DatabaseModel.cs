using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;



namespace Prog_Web_Application.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        
        public string Role { get; set; }
        public ICollection<Claim> Claims { get; set; }
    }

    // Claim model
    public class Claim
    {
        [Key]
        [Required(ErrorMessage = "This stupid thing being an int is causing the error")]
        public int ClaimID { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address such as JohnDoe@mail.com")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [Phone(ErrorMessage = "Please enter a valid phone number such as 123-456-7890")]
        [StringLength(15)]
        public string Phone { get; set; }

        [Required]
        [StringLength(200)]
        public string ClaimDescription { get; set; }

        public byte[]? UploadedFile { get; set; }

        [Required]
        public int HoursWorked { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Hourly rate must be greater than 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal HourlyRate { get; set; }

        [Required]
        public DateTime SubmissionDate { get; set; }

        [StringLength(100)]
        public string? FileName { get; set; }

        [Required]
        public ClaimStatus Status { get; set; } = ClaimStatus.Pending;

        [StringLength(200)]
        public string? AdditionalNotes { get; set; }

        [NotMapped]
        public decimal TotalAmount => HourlyRate * HoursWorked;
    }

    // Claim Status Enum
    public enum ClaimStatus
    {
        Pending,
        Approved,
        Rejected
    }
}