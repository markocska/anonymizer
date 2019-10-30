using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Scrambler.Quartz.ConfigProviders
{
    interface ISchedulerConfigurationProvider
    {
        NameValueCollection SchedulerConfiguration { get; set; }
    }
}
