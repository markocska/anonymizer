using System;
using System.Collections.Generic;
using System.Text;
using Serilog;

namespace Scrambler.Quartz.ConfigProviders
{
    public class LoggerConfigurationProvider : ILoggerConfigurationProvider
    {
        public LoggerConfiguration LoggerConfiguration { get; set; }
    }
}
