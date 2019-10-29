using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Scrambler.Quartz
{
    public class SchedulingService
    {

        private ISchedulerFactory _schedulerFactory;
        private IScheduler _scheduler;
        private LoggerConfiguration _loggerConfig;

        public SchedulingService(LoggerConfiguration loggerConfig, NameValueCollection schedulerConfig)
        {
            _schedulerFactory = new StdSchedulerFactory(schedulerConfig);
            _loggerConfig = loggerConfig;
            Log.Logger = loggerConfig.CreateLogger(); 
        }

        public ScheduleScramblingJob()
        {

        }
    }
}
