using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.Quartz.JobFactory
{
    // source reference: https://stackoverflow.com/questions/42157775/net-core-quartz-dependency-injection/42158004#42158004
    public class ScramblerJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<IJob, IServiceScope> _scopes = new ConcurrentDictionary<IJob, IServiceScope>();

        public ScramblerJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
                var scope = _serviceProvider.CreateScope();
                IJob job;

                try
                {
                    job = scope.ServiceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
                 
                }
                catch
                {
                    scope.Dispose();
                    throw;
                }

                if (!_scopes.TryAdd(job, scope))
                {
                    scope.Dispose();
                    throw new Exception("Failed to track DI scope");
                }

                return job;
        }

        public void ReturnJob(IJob job)
        {
            if (_scopes.TryRemove(job, out var scope))
            {
                scope.Dispose();
            }
        }

    }
}
