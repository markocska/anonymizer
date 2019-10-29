using Microsoft.Extensions.Logging;
using Scrambler.SqlServer;
using Serilog;
using System;
using System.IO;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = File.ReadAllText(".\\scrambleConfig.json");

            var logConfig = new LoggerConfiguration()
                .WriteTo.Console();

            var scrambler = new SqlScramblingService(logConfig);
            scrambler.ScrambleFromConfigStr(config);

            Console.ReadKey();
        }
    }
}
