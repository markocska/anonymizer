using Quartz;
using System;
using System.Threading.Tasks;

namespace Scrambler.Quartz
{
    public class SqlScramblingJob : IJob
    {

        public string ConfigStr { get; set; }

        public Task Execute(IJobExecutionContext context)
        {
            var jobDataMap = context.MergedJobDataMap;

            var logConfig = jobDataMap.Get("logConfig")

           
        }
    }
}
