using Anonymizer.Config;
using Anonymizer.DbServices;
using Anonymizer.Factories;
using Anonymizer.Initializers;
using Serilog;
using System;

namespace Anonymizer
{
    public class Program
    {

        private static readonly ILogger log;
        static Program()
        {
            SerilogInitializer.Register();

            log = Serilog.Log.ForContext<Program>();
        }

        static void Main(string[] args)
        {
            try
            {   
                DatabaseConfigurationSection config =
                    System.Configuration.ConfigurationManager.GetSection(DatabaseConfigurationSection.SectionName) as DatabaseConfigurationSection;

                var tableInfoList = TableInfoCollectionFactory.CreateTableListFromConfig(config);

                foreach (var tableInfo in tableInfoList)
                {
                    ScrambleManager.Scramble(tableInfo);
                }

                Log.CloseAndFlush();
            } 
            catch (Exception ex)
            {
                log.Fatal(ex, "The anonymizing could not be done, because there was a fatal error: " + ex.Message);
                Log.CloseAndFlush();
            }

        }
    }
}
