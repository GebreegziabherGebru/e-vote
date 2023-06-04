using CandidateManagementService.Dtos;
using CandidateManagementService.Models;
using AutoMapper;

namespace CandidateManagementService.Profiles
{
    public class CandidatesProfile : Profile
    {
        public CandidatesProfile()
        {
            //Source -> Target
            CreateMap<Candidate, CandidateResponseDto>();
            CreateMap<CandidateRequestDto, Candidate>();
        }
    }
}