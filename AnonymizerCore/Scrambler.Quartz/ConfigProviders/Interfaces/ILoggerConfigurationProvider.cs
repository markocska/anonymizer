using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.Quartz.ConfigProviders
{
    public interface ILoggerConfigurationProvider
    {
        LoggerConfiguration LoggerConfiguration { get; set; }
    }
}
