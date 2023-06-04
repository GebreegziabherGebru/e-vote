using CandidateManagementService.Dtos;

namespace CandidateManagementService.Repository
{
    public interface ICandidateRepository
    {
        Task<CandidateResponseDto> FindByIdAsync(long id);
        Task<IEnumerable<CandidateResponseDto>> FindAllAsync();
        Task<CandidateResponseDto> SaveAsync(CandidateRequestDto requestDto);
        Task DeleteByIdAsync(long id);
        Task DeleteAllAsync();
        Task<CandidateResponseDto> UpdateAsync(long id, CandidateRequestDto requestDto);
    }
}
