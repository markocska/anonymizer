using AutoMapper;
using Quartz;
using Scrambler.Quartz.Model;
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

            CreateMap<TriggerKeyWithDescription, Scrambler.Api.Dtos.TriggerDescription>(MemberList.None)
                .ForMember(x => x.TriggerName, opt => opt.MapFrom(x => x.TriggerName))
                .ForMember(x => x.TriggerGroup, opt => opt.MapFrom(x => x.TriggerGroup))
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.Description))
                .ForMember(x => x.CalendarName, opt => opt.MapFrom(x => x.CalendarName));

            CreateMap<JobKeyWithDescription, Scrambler.Api.Dtos.JobDescription>(MemberList.None)
                 .ForMember(x => x.Id, opt => opt.MapFrom(x => x.JobGroup + x.JobName))
                 .ForMember(x => x.JobName, opt => opt.MapFrom(x => x.JobName))
                 .ForMember(x => x.JobGroup, opt => opt.MapFrom(x => x.JobGroup))
                 .ForMember(x => x.RequestRecovery, opt => opt.MapFrom(x => x.RequestRecovery))
                 .ForMember(x => x.Description, opt => opt.MapFrom(x => x.Description))
                 .ForMember(x => x.IsDurable, opt => opt.MapFrom(x => x.IsDurable))
                 .ForMember(x => x.Triggers, opt => opt.MapFrom(x => x.Triggers));
        }

    }
}