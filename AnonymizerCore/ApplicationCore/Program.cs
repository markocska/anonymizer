using ApplicationCore.Config;
using ApplicationCore.DatabaseServices.ColumnTypes;
using ApplicationCore.DatabaseServices.Indexes;
using ApplicationCore.DatabaseServices.PrimaryKeys;
using ApplicationCore.DatabaseServices.Scrambling;
using ApplicationCore.Factories;
using ApplicationCore.Logging;
using ApplicationCore.ServiceProviders;
using ApplicationCore.SqlScripts.SqlServer;
using ApplicationCore.TableInfo;
using ApplicationCore.TableInfo.Interfaces;
using ApplicationCore.Validators.ConfigValidators;
using ApplicationCore.Validators.ParameterValidators;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ApplicationCore
{
    class Program
    {
        static Program()
        {
            SerilogInitializer.Register();
        }

        static void Main(string[] args)
        {

            var config = File.ReadAllText(".\\tablesConfig.json");
            var databasesConfig = JsonConvert.DeserializeObject<DatabasesConfig>(config);

            var serviceProvider = SqlServiceProvider.GetSqlServiceProvider();


            var tableInfoCollectionFactory = serviceProvider.GetService<ITableInfoCollectionFactory>();
            var validTables = tableInfoCollectionFactory.CreateTableListFromConfig(databasesConfig);

            var scramblingService = new SqlScramblingService();
            foreach (var table in validTables)
            {
                var scramblingScript = scramblingService.GenerateScramblingScript(table);
                Console.WriteLine(scramblingScript);
            }

            Console.ReadKey();
        }
    }
}