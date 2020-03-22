using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Scrambler.Config;
using Scrambler.Quartz.Configuration;
using Scrambler.Quartz.Interfaces;
using Scrambler.Quartz.JobFactory;
using Scrambler.Quartz.Jobs;
using Scrambler.Quartz.Model;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrambler.Quartz
{
    public class SchedulingService : ISchedulingService
    {
        public IScheduler _scheduler { get; set; }

        public IMapper _mapper { get; set; }
        public SchedulingService(LoggerConfiguration loggerConfiguration, SchedulerConfiguration schedulerConfig, IMapper mapper)
        {
            Log.Logger = loggerConfiguration.CreateLogger();

            _mapper = mapper;

            var services = new ServiceCollection();
            services.AddTransient(s => loggerConfiguration);
            services.AddTransient(s => schedulerConfig);
            services.AddTransient<SqlScramblingJob>();
            services.AddTransient<MySqlScramblingJob>();
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

            foreach (var jobKey in jobKeys)
            {
                var jobDetail = await _scheduler.GetJobDetail(jobKey);
                var triggers = await _scheduler.GetTriggersOfJob(jobKey);

                jobKeysWithDescription.Add(
                    new JobKeyWithDescription
                    {
                        JobName = jobKey.Name,
                        JobGroup = jobKey.Group,
                        Description = jobDetail.Description,
                        RequestRecovery = jobDetail.RequestsRecovery,
                        IsDurable = jobDetail.Durable,
                        Triggers = _mapper.Map<List<ITrigger>, List<TriggerKeyWithDescription>>(triggers.ToList())
                    });
            }

            return jobKeysWithDescription;
        }

        public async Task<List<JobKeyWithDescription>> GetJobKeysWithDescription(string groupName)
        {
            var jobKeys = await _scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupContains(groupName));

            var jobKeysWithDescription = new List<JobKeyWithDescription>();

            foreach (var jobKey in jobKeys)
            {
                var jobDetail = await _scheduler.GetJobDetail(jobKey);
                var triggers = await _scheduler.GetTriggersOfJob(jobKey);

                jobKeysWithDescription.Add(
                    new JobKeyWithDescription
                    {
                        JobName = jobKey.Name,
                        JobGroup = jobKey.Group,
                        Description = jobDetail.Description,
                        RequestRecovery = jobDetail.RequestsRecovery,
                        IsDurable = jobDetail.Durable,
                        Triggers = _mapper.Map<List<ITrigger>, List<TriggerKeyWithDescription>>(triggers.ToList())
                    });
            }

            return jobKeysWithDescription;
        }

        public async Task<bool> DeleteJob(string jobName, string jobGroup)
        {
            var wasJobFoundAndDeleted = await _scheduler.DeleteJob(new JobKey(jobName, jobGroup));
            return wasJobFoundAndDeleted;
        }

        public async Task<bool> DeleteTrigger(string triggerName, string triggerGroup)
        {
            var wasTriggerFoundAndDeleted = await _scheduler.UnscheduleJob(new TriggerKey(triggerName, triggerGroup));
            return wasTriggerFoundAndDeleted;
        }

        public async Task<SchedulingResult> RescheduleJob(string triggerGroup, string triggerName, string cronExpression, string triggerDescription)
        {
            if (!CronExpression.IsValidExpression(cronExpression))
            {
                return new SchedulingResult { IsSuccessful = false, ErrorMessage = "The cron expression is invalid." };
            }

            var associatedTrigger = (await _scheduler.GetTrigger(new TriggerKey(triggerName, triggerGroup)));
            if (associatedTrigger == null)
            {
                return new SchedulingResult { IsSuccessful = false, ErrorMessage = "No schedule was found with the given key." };
            }

            // creating copies before unscheduling
            var associatedJobKey = associatedTrigger.JobKey;
            var associatedJobCopy = (await _scheduler.GetJobDetail(associatedJobKey)).GetJobBuilder().Build();
            var associatedTriggerCopyModified = associatedTrigger.GetTriggerBuilder()
                .WithDescription(triggerDescription)
                .WithCronSchedule(cronExpression)
                .Build();

            var wasTriggerFoundAndUnscheduled = await _scheduler.UnscheduleJob(new TriggerKey(triggerName, triggerGroup));

            if (!wasTriggerFoundAndUnscheduled)
            {
                return new SchedulingResult { IsSuccessful = false, ErrorMessage = "Couldn't unschedule the schedule." };
            }

            await _scheduler.ScheduleJob(associatedJobCopy, associatedTriggerCopyModified);

            return new SchedulingResult { IsSuccessful = true };
        }

        public async Task<SchedulingResult> ScheduleSqlScramblingJob(string jobName, string jobGroup, string triggerDescription,
            string cronExpression, string description, DatabasesConfig jobConfig)
        {
            return (await ScheduleScramblingJob<SqlScramblingJob>(jobName, jobGroup, triggerDescription, cronExpression, description, jobConfig));
        }

        public async Task<SchedulingResult> ScheduleMySqlScramblingJob(string jobName, string jobGroup, string triggerDescription,
           string cronExpression, string description, DatabasesConfig jobConfig)
        {
            return (await ScheduleScramblingJob<MySqlScramblingJob>(jobName, jobGroup, triggerDescription, cronExpression, description, jobConfig));
        }

        private async Task<SchedulingResult> ScheduleScramblingJob<JobType>(string jobName, string jobGroup, string triggerDescription,
           string cronExpression, string description, DatabasesConfig jobConfig)
            where JobType : IJob
        {
            if (!CronExpression.IsValidExpression(cronExpression))
            {
                return new SchedulingResult { IsSuccessful = false, ErrorMessage = "The cron expression is invalid." };
            }
            var jobDetail = _scheduler.GetJobDetail(new JobKey(jobName, jobGroup));
            if (_scheduler.GetJobDetail(new JobKey(jobName, jobGroup)).Status != TaskStatus.WaitingForActivation)
            {
                return new SchedulingResult { IsSuccessful = false, ErrorMessage = "The jobname is already taken." };
            }

            var job = JobBuilder.CreateForAsync<JobType>()
                .WithIdentity(jobName, jobGroup)
                .UsingJobData("configStr", JsonConvert.SerializeObject(jobConfig))
                .WithDescription(description)
                .RequestRecovery()
                .Build();

            var trigger = (ICronTrigger)TriggerBuilder.Create()
                .WithDescription(triggerDescription)
                .WithCronSchedule(cronExpression)
                .Build();

            //Console.WriteLine($"Trigger before: name: {trigger.Key.Name} group: {trigger.Key.Group}" );
            try
            {
                await _scheduler.ScheduleJob(job, trigger);
                //Console.WriteLine($"Trigger after: name: {trigger.Key.Name} group: {trigger.Key.Group}");
            }
            catch (ObjectAlreadyExistsException ex)
            {
                return new SchedulingResult
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message
                };
            }
            catch (JobPersistenceException ex)
            {
                return new SchedulingResult
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message
                };
            }

            return new SchedulingResult { IsSuccessful = true, JobKey = job.Key, TriggerKey = trigger.Key };
        }


    }
}

