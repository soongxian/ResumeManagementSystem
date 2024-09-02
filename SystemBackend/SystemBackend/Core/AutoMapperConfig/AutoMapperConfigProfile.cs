using AutoMapper;
using SystemBackend.Core.Dtos.Candidate;
using SystemBackend.Core.Dtos.Company;
using SystemBackend.Core.Dtos.Job;
using SystemBackend.Core.Entities;

namespace SystemBackend.Core.AutoMapperConfig
{
    public class AutoMapperConfigProfile : Profile
    {
        public AutoMapperConfigProfile() 
        {
            //Company
            CreateMap<CompanyCreateDto, Company>();
            CreateMap<Company, CompanyGetDto>();

            // Job
            CreateMap<JobCreateDto, Job>();
            CreateMap<Job, JobGetDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name));
            
            // Candidate
            CreateMap<CandidateCreateDto,Candidate>();
            CreateMap<Candidate,CandidateGetDto>()
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src =>src.Job.Title));
        }
    }
}
