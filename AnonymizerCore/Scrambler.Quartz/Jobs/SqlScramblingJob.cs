using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Logging;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Scrambler.Quartz.Jobs
{
    public class SqlScramblingJob : IJob
    {
  
        public string ConfigStr { get; set; }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("It works!");

            Log.Information("It double works");
           
        }
    }
}
