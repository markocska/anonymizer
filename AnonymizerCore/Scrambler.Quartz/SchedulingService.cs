using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.AdoJobStore;
using Quartz.Impl.Matchers;
using Quartz.Logging;
using Scrambler.Quartz.ConfigProviders;
using Scrambler.Quartz.Configuration;
using Scrambler.Quartz.Interfaces;
using Scrambler.Quartz.JobFactory;
using Scrambler.Quartz.Jobs;
using Scrambler.Quartz.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrambler.Quartz
{
    public class SchedulingService : ISchedulingService
    {
        public IScheduler _scheduler { get; set; }


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

        public async Task<IReadOnlyCollection<JobKey>> GetAllJobKeys()
        {
            var jobKeys = await _scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());

            return jobKeys;
        }

        public async Task<List<JobKeyWithDescription>> GetAllJobKeysWithDescription()
        {
            var jobKeys = await _scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());

            var jobKeysWithDescription = new List<JobKeyWithDescription>();
            
            foreach (var jobkey in jobKeys)
            {
                var jobDetail = await _scheduler.GetJobDetail(jobkey);

                jobKeysWithDescription.Add(
                    new JobKeyWithDescription
                    {
                        JobName = jobkey.Name,
                        JobGroup = jobkey.Group,
                        Description = jobDetail.Description,
                        RequestRecovery = jobDetail.RequestsRecovery
                    });
            }

            return jobKeysWithDescription;
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

            var job = JobBuilder.CreateForAsync<SqlScramblingJob>()
                .WithIdentity(jobName, jobGroup)
                .WithDescription(description)
                .RequestRecovery()
                .Build();

            var trigger = (ICronTrigger) TriggerBuilder.Create()
                .WithIdentity(triggerName, triggerGroup)
                .WithCronSchedule(cronExpression)
                .Build();

            try
            {
                await _scheduler.ScheduleJob(job, trigger);
            }
            catch (ObjectAlreadyExistsException ex) 
            {
                return new SchedulingResult
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message
                };
            }

            return new SchedulingResult { IsSuccessful = true };
        }

    }
}

