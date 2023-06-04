using AutoMapper;
using CandidateManagementService.Context;
using CandidateManagementService.Dtos;
using CandidateManagementService.Models;
using Microsoft.EntityFrameworkCore;

namespace CandidateManagementService.Repository
{
    public class CandidateRepository : ICandidateRepository
    {

        private CandidatesManagementDbContext context;
        private IMapper mapper;

        public CandidateRepository(CandidatesManagementDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<CandidateResponseDto> FindByIdAsync(long id)
        {
            var candidate = await context.candidates.FindAsync(id);
            return mapper.Map<CandidateResponseDto>(candidate);
        }
        public async Task<IEnumerable<CandidateResponseDto>> FindAllAsync()
        {
            var candidates = await context.candidates.ToListAsync();
            return mapper.Map<IEnumerable<CandidateResponseDto>>(candidates);
        }
        public async Task<CandidateResponseDto> SaveAsync(CandidateRequestDto requestDto)
        {
            var candidateEntity = await context.AddAsync(mapper.Map<Candidate>(requestDto));
            await context.SaveChangesAsync();
            return mapper.Map<CandidateResponseDto>(candidateEntity.Entity);
        }
        public async Task DeleteByIdAsync(long id)
        {
            context.candidates.Remove(new Candidate() { Id = id });
            await context.SaveChangesAsync();
        }
        public async Task DeleteAllAsync()
        {
            context.candidates.RemoveRange(context.candidates);
            await context.SaveChangesAsync();
        }
        public async Task<CandidateResponseDto> UpdateAsync(long id, CandidateRequestDto requestDto)
        {
            var existingCandidate = await context.candidates.FindAsync(id);

            if (existingCandidate == null)
                throw new Exception("Candidate not found.");
            mapper.Map(requestDto, existingCandidate);
            await context.SaveChangesAsync();
            return mapper.Map<CandidateResponseDto>(existingCandidate);
        }
    }
}
