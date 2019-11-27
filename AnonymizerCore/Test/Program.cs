using Microsoft.Extensions.Logging;
using Scrambler.MySql;
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
            //var config = File.ReadAllText(".\\scrambleConfig.json");

            //var scrambler = new SqlScramblingService(logConfig);
            //scrambler.ScrambleFromConfigStr(config);

            var logConfig = new LoggerConfiguration()
                .WriteTo.Console();

            var mySqlconfig = File.ReadAllText(".\\mySqlscrambleConfig.json");

            var mySqlScrambler = new MySqlScramblingService(logConfig);
            mySqlScrambler.ScrambleFromConfigStr(mySqlconfig);

            Console.ReadKey();
        }
    }
}
