using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Logging;
using Scrambler.Quartz.Configuration;
using Scrambler.SqlServer;
using Serilog;
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
            Log.Information($"Job execution starting: Description: {context.JobDetail.Description}, " +
                $"Groupname: {context.JobDetail.Key.Group}, Keyname: {context.JobDetail.Key.Name}.");

            try
            {
                string configStr = context.MergedJobDataMap.GetString("configStr");
                var scrambler = new SqlScramblingService(_loggerConfiguration);
                scrambler.ScrambleFromConfigStr(configStr);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while executing job:  Description: {context.JobDetail.Description}, " +
                $"Groupname: {context.JobDetail.Key.Group}, Keyname: {context.JobDetail.Key.Name}.");
            }

            Log.Information($"Job execution successfully finished: Description: {context.JobDetail.Description}, " +
                $"Groupname: {context.JobDetail.Key.Group}, Keyname: {context.JobDetail.Key.Name}.");
           
        }
    }
}
