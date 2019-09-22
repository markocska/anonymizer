using Microsoft.Extensions.Logging;
using Scrambler.SqlServer;
using System;
using System.IO;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = File.ReadAllText(".\\tablesConfig.json");

            Action<ILoggingBuilder> logConfig = logBuilder => logBuilder.AddConsole();

            var scrambler = new SqlScramblingService(logConfig);
            scrambler.ScrambleFromConfigStr(config);

            Console.ReadKey();
        }
    }
}
