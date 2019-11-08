using AutoMapper;
using Quartz;

namespace Scrambler.Api.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<JobKey, Scrambler.Api.Dtos.JobKey>(MemberList.None)
            .ForMember(x => x.Group, opt => opt.MapFrom(x => x.Group))
            .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name));
        }

    }
}