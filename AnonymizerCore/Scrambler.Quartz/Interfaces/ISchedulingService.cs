﻿using Quartz;
using Scrambler.Quartz.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scrambler.Quartz.Interfaces
{
    public interface ISchedulingService
    {
        Task<SchedulingResult> ScheduleSqlScramblingJob(string jobName, string jobGroup, string triggerName, string triggerGroup,
            string cronExpression, string description);

        Task<IReadOnlyCollection<JobKey>> GetAllJobKeys();

        Task<List<JobKeyWithDescription>> GetAllJobKeysWithDescription();
    }
}
