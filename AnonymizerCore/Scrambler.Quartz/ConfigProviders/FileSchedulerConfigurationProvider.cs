using Newtonsoft.Json;
using Scrambler.Quartz.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace Scrambler.Quartz.ConfigProviders
{
    public class FileSchedulerConfigurationProvider
    {
        public static SchedulerConfiguration GetSchedulerConfig(string schedulerConfigPath)
        {
            var schedulerConfig = File.ReadAllText(schedulerConfigPath);
            if (string.IsNullOrEmpty(schedulerConfig))
            {
                throw new SchedulerConfigException("The scheduler config file can't be empty.");
            }

            var schedulerConfigDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(schedulerConfig);
            if (schedulerConfigDict == null)
            {
                throw new SchedulerConfigException("The scheduler config couldn't be parsed.");
            }

            var config = new NameValueCollection();
            foreach (var keyValuePair in schedulerConfigDict)
            {
                config.Add(keyValuePair.Key, keyValuePair.Value);
            }

            return new SchedulerConfiguration
            {
                NameValueCollection = config
            };
        }
    }
}
