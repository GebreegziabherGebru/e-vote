using CandidateManagementService.Models;
using Microsoft.EntityFrameworkCore;

namespace CandidateManagementService.Context
{
    public class CandidatesManagementDbContext : DbContext
    {
        public CandidatesManagementDbContext(DbContextOptions<CandidatesManagementDbContext> opts)
        : base(opts) { }
        public DbSet<Candidate> candidates { get; set; }
    }
}