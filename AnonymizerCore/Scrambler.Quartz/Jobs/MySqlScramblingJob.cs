using Quartz;
using Scrambler.MySql;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scrambler.Quartz.Jobs
{
    public class MySqlScramblingJob : IJob
    {
        private readonly LoggerConfiguration _loggerConfiguration;

        public MySqlScramblingJob(LoggerConfiguration loggerConfiguration)
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
                var scrambler = new MySqlScramblingService(_loggerConfiguration);
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
