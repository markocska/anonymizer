﻿using Quartz;
using Scrambler.Config;
using Scrambler.Quartz.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scrambler.Quartz.Interfaces
{
    public interface ISchedulingService
    {
        Task<SchedulingResult> ScheduleSqlScramblingJob(string jobName, string jobGroup, string triggerDescription,
            string cronExpression, string description, DatabasesConfig jobConfig);
        Task<SchedulingResult> ScheduleMySqlScramblingJob(string jobName, string jobGroup, string triggerDescription,
            string cronExpression, string description, DatabasesConfig jobConfig);
        Task<SchedulingResult> RescheduleJob(string triggerGroup, string triggerName, string cronExpression, string triggerDescription);
        Task<SchedulingResult> AddJobSchedule(string jobGroup, string jobName, string cronExpression, string triggerDescription);
        Task<IReadOnlyCollection<JobKey>> GetAllJobKeys();
        Task<List<JobKeyWithDescription>> GetAllJobKeysWithDescription();
        Task<List<JobKeyWithDescription>> GetJobKeysWithDescription(string groupName);
        Task<bool> DeleteJob(string jobName, string jobGroup);
        Task<bool> DeleteTrigger(string triggerName, string triggerGroup);
        Task<IEnumerable<string>> GetAllJobGroups();
        Task<IEnumerable<string>> GetAllJobKeysForJobGroup(string jobGroup);

        void StartScheduler();
    }
}
