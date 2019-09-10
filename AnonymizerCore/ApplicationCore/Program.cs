using Scrambler.Config;
using Scrambler.DatabaseServices.ColumnTypes;
using Scrambler.DatabaseServices.Indexes;
using Scrambler.DatabaseServices.PrimaryKeys;
using Scrambler.DatabaseServices.Scrambling;
using Scrambler.Factories;
using Scrambler.Logging;
using Scrambler.TableInfo;
using Scrambler.TableInfo.Interfaces;
using Scrambler.Validators.ConfigValidators;
using Scrambler.Validators.ParameterValidators;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Scrambler
{
    class Program
    {
        static Program()
        {
            SerilogInitializer.Register();
        }

        static void Main(string[] args)
        {

            //var config = File.ReadAllText(".\\tablesConfig.json");
            //var databasesConfig = JsonConvert.DeserializeObject<DatabasesConfig>(config);

            //var serviceProvider = SqlServiceProvider.GetSqlServiceProvider();


            //var tableInfoCollectionFactory = serviceProvider.GetService<ITableInfoCollectionFactory>();
            //var validTables = tableInfoCollectionFactory.CreateTableListFromConfig(databasesConfig);

            //var scramblingService = new SqlScramblingService();
            //foreach (var table in validTables)
            //{
            //    var scramblingScript = scramblingService.GenerateScramblingScript(table);
            //    Console.WriteLine(scramblingScript);
            //}

            Console.ReadKey();
        }
    }
}