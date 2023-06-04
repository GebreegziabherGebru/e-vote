using System.ComponentModel.DataAnnotations;

namespace CandidateManagementService.Dtos
{
    public class CandidateRequestDto
    {
        [Required]
        [MaxLength(30)]
        [MinLength(2)]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(30)]
        [MinLength(2)]
        public string? LastName { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }

        [Required]
        [MinLength(10)]
        public string? Biography { get; set; }
        public Status Status { get; set; }
    }
}