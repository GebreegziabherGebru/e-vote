namespace CandidateManagementService.Dtos
{
    public class CandidateResponseDto
    {
        public long Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Biography { get; set; }
        public Status Status { get; set; }
    }
}