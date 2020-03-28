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
                .ForMember(x => x.CalendarName, opt => opt.MapFrom(x => x.CalendarName))
                .ForMember(x => x.CronExpression, opt =>
                {
                    opt.PreCondition((ITrigger src) => src is ICronTrigger);
                    opt.MapFrom(src => ((ICronTrigger)src).CronExpressionString);
                });

            CreateMap<TriggerKeyWithDescription, Scrambler.Api.Dtos.TriggerDescription>(MemberList.None)
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.TriggerGroup + x.TriggerName))
                .ForMember(x => x.TriggerName, opt => opt.MapFrom(x => x.TriggerName))
                .ForMember(x => x.TriggerGroup, opt => opt.MapFrom(x => x.TriggerGroup))
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.Description))
                .ForMember(x => x.CalendarName, opt => opt.MapFrom(x => x.CalendarName))
                .ForMember(x => x.CronExpression, opt => opt.MapFrom(x => x.CronExpression));

            CreateMap<JobKeyWithDescription, Scrambler.Api.Dtos.JobDescription>(MemberList.None)
                 .ForMember(x => x.Id, opt => opt.MapFrom(x => x.JobGroup + x.JobName))
                 .ForMember(x => x.JobName, opt => opt.MapFrom(x => x.JobName))
                 .ForMember(x => x.JobGroup, opt => opt.MapFrom(x => x.JobGroup))
                 .ForMember(x => x.RequestRecovery, opt => opt.MapFrom(x => x.RequestRecovery))
                 .ForMember(x => x.Description, opt => opt.MapFrom(x => x.Description))
                 .ForMember(x => x.IsDurable, opt => opt.MapFrom(x => x.IsDurable))
                 .ForMember(x => x.Triggers, opt => opt.MapFrom(x => x.Triggers));

            CreateMap<Scrambler.Quartz.SchedulingResult, Scrambler.Api.Dtos.TriggerSuccessfullyCreated>(MemberList.None)
             .ForMember(x => x.Id, opt => opt.MapFrom(x => x.TriggerKey.Group + x.TriggerKey.Name))
             .ForMember(x => x.JobGroup, opt => opt.MapFrom(x => x.JobKey.Group))
             .ForMember(x => x.JobName, opt => opt.MapFrom(x => x.JobKey.Name))
             .ForMember(x => x.TriggerGroup, opt => opt.MapFrom(x => x.TriggerKey.Group))
             .ForMember(x => x.TriggerName, opt => opt.MapFrom(x => x.TriggerKey.Name))
             .ForMember(x => x.TriggerDescription, opt => opt.MapFrom(x => x.TriggerDescription))
             .ForMember(x => x.CronExpression, opt => opt.MapFrom(x => x.CronExpression))
             .ForMember(x => x.Calendar, opt => opt.MapFrom(x => x.Calendar));
        }

    }
}