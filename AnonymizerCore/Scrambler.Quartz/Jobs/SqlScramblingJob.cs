using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Logging;
using Scrambler.Quartz.Configuration;
using Scrambler.SqlServer;
using Serilog;
using Serilog.Context;
using System;
using System.Threading.Tasks;


namespace Scrambler.Quartz.Jobs
{
    public class SqlScramblingJob : IJob
    {
        private readonly LoggerConfiguration _loggerConfiguration;

        public SqlScramblingJob(LoggerConfiguration loggerConfiguration)
        {
            _loggerConfiguration = loggerConfiguration;
        }

        public string ConfigStr { get; set; }

        public async Task Execute(IJobExecutionContext context)
        {
            Log.Information("Job execution starting: Description: {JobDescription}, " +
                "Groupname: {GroupKey}, Keyname: {JobKey}.", context.JobDetail.Description,
                    context.JobDetail.Key.Group, context.JobDetail.Key.Name);

            try
            {
                using (LogContext.PushProperty("JobKey", context.JobDetail.Key.Name))
                using (LogContext.PushProperty("GroupKey", context.JobDetail.Key.Group))
                using (LogContext.PushProperty("JobDescription", context.JobDetail.Description))
                {
                    string configStr = context.MergedJobDataMap.GetString("configStr");
                    var scrambler = new SqlScramblingService(_loggerConfiguration);
                    scrambler.ScrambleFromConfigStr(configStr);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while executing job:  Description: {JobDescription}, " +
                 "Groupname: {GroupKey}, Keyname: {JobKey}.",
                    context.JobDetail.Description, context.JobDetail.Key.Group, context.JobDetail.Key.Name);
            }

            Log.Information("Job execution successfully finished: Description: {JobDescription}, " +
                "Groupname: {GroupKey}, Keyname: {JobKey}.", context.JobDetail.Description,
                context.JobDetail.Key.Group, context.JobDetail.Key.Name);

            
           
        }
    }
}
