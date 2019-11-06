using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.AdoJobStore;
using Quartz.Logging;
using Scrambler.Quartz.ConfigProviders;
using Scrambler.Quartz.Configuration;
using Scrambler.Quartz.Interfaces;
using Scrambler.Quartz.JobFactory;
using Scrambler.Quartz.Jobs;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;

namespace Scrambler.Quartz
{
    public class SchedulingService : ISchedulingService
    {
        private IScheduler _scheduler;


        public SchedulingService(LoggerConfiguration loggerConfiguration, SchedulerConfiguration schedulerConfig)
        {
            Log.Logger = loggerConfiguration.CreateLogger();

            var services = new ServiceCollection();
            services.AddTransient(s => loggerConfiguration);
            services.AddTransient(s => schedulerConfig);
            services.AddTransient<SqlScramblingJob>();
            var container = services.BuildServiceProvider();

            var jobFactory = new ScramblerJobFactory(container);

            var schedulerFactory = new StdSchedulerFactory(schedulerConfig.NameValueCollection);
            var scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();

            scheduler.JobFactory = jobFactory;

            _scheduler = scheduler;

            _scheduler.Start();
        }

        public async Task<SchedulingResult> ScheduleSqlScramblingJob(string jobName, string jobGroup, string triggerName, string triggerGroup,
            string cronExpression, string description)
        {
            if (!CronExpression.IsValidExpression(cronExpression))
            {
                return new SchedulingResult {IsSuccessful = false, ErrorMessage = "The cron expression is invalid." };
            }
            var jobDetail = _scheduler.GetJobDetail(new JobKey(jobName, jobGroup));
            if (_scheduler.GetJobDetail(new JobKey(jobName, jobGroup)).Status != TaskStatus.WaitingForActivation)
            {
                return new SchedulingResult { IsSuccessful = false, ErrorMessage = "The jobname is already taken." };
            }

            //var triggerStatus = _scheduler.GetTrigger(new TriggerKey(triggerName, triggerGroup));
            //if (_scheduler.GetTrigger(new TriggerKey(triggerName, triggerGroup)). != TaskStatus.WaitingForActivation )
            //{
            //    return new SchedulingResult { IsSuccessful = false, ErrorMessage = "The triggername is already taken." };
            //}

            var job = JobBuilder.CreateForAsync<SqlScramblingJob>()
                .WithIdentity(jobName, jobGroup)
                .WithDescription(description)
                .RequestRecovery()
                .Build();

            var trigger = (ICronTrigger) TriggerBuilder.Create()
                .WithIdentity(triggerName, triggerGroup)
                .WithCronSchedule(cronExpression)
                .Build();

            await _scheduler.ScheduleJob(job, trigger);

            return new SchedulingResult { IsSuccessful = true };
        }

    }
}

