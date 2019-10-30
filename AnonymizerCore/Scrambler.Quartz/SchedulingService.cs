using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using Scrambler.Quartz.ConfigProviders;
using Scrambler.Quartz.Configuration;
using Scrambler.Quartz.JobFactory;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;

namespace Scrambler.Quartz
{
    public class SchedulingService
    {
        private IScheduler _scheduler;

        public SchedulingService(LoggerConfiguration loggerConfiguration, SchedulerConfiguration schedulerConfig)
        {
            var services = new ServiceCollection();
            services.AddTransient(s => loggerConfiguration);
            services.AddTransient(s => schedulerConfig);
            var container = services.BuildServiceProvider();

            var jobFactory = new ScramblerJobFactory(container);

            var schedulerFactory = new StdSchedulerFactory(schedulerConfig.Config);
            var scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();

            scheduler.JobFactory = jobFactory;

            _scheduler = scheduler;
        }

        //public async Task ScheduleSqlScramblingJob(string scramblingConfig, string cronExpression,string description)
        
        //    var job = JobBuilder.CreateForAsync<SqlScramblingJob>()
        //        .WithDescription(description)
        //        .UsingJobData(nameof(SqlScramblingJob.ConfigStr),scramblingConfig)
        //        .
        //        .Build();

            
        //}
    }
}
