using CandidateManagementService.Models;

namespace CandidateManagementService.Context
{
    public class InitialDataSeeder
    {
        private CandidatesManagementDbContext context;

        public InitialDataSeeder(CandidatesManagementDbContext dataContext)
        {
            context = dataContext;
        }

        public void SeedData()
        {
            if (!context.candidates.Any())
            {
                var candidates = new List<Candidate>
                {
                    new Candidate { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890", Biography = "I am running for presidency with a vision to create positive change and prosperity for our nation.", Status = Status.PENDING },
                    new Candidate { FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", PhoneNumber = "9876543210", Biography = "As a candidate for presidency, I am committed to championing the rights of every citizen and ensuring a brighter future for our country.", Status = Status.PENDING },
                };

                context.candidates.AddRange(candidates);
                context.SaveChanges();
            }
        }
    }
}