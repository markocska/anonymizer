using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Scrambler.Api.Config
{
    public class ConfigurationLoader
    {
        public static string GetLogsDbConnectionString(IConfiguration configuration)
        {
            var msSqlSerilogConfig = configuration.GetSection("Serilog").GetSection("WriteTo").GetChildren().Where(c => c.GetValue<string>("Name") == "MSSqlServer").FirstOrDefault();

            if (msSqlSerilogConfig == null)
            {
                throw new ConfigurationErrorsException("No valid serilog config for dbConnectionString");
            }

            var connectionString = msSqlSerilogConfig.GetSection("Args").GetValue<string>("connectionString");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ConfigurationErrorsException("No valid serilog config for dbConnectionString");
            }

            return connectionString;
        }
    }
}
