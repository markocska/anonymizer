using AutoMapper;
using Quartz;
using System.Collections.Generic;

namespace Scrambler.Api.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<JobKey, Scrambler.Api.Dtos.JobKey>(MemberList.None)
            .ForMember(x => x.Group, opt => opt.MapFrom(x => x.Group))
            .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name));

            CreateMap<ITrigger, Quartz.Model.TriggerKeyWithDescription>(MemberList.None)
                .ForMember(x => x.TriggerGroup, opt => opt.MapFrom(x => x.Key.Group))
                .ForMember(x => x.TriggerName, opt => opt.MapFrom(x => x.Key.Name))
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.Description))
                .ForMember(x => x.CalendarName, opt => opt.MapFrom(x => x.CalendarName));

                
        }

    }
}