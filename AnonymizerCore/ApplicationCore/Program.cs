using ApplicationCore.Config;
using ApplicationCore.DatabaseServices.ColumnTypes;
using ApplicationCore.DatabaseServices.PrimaryKeys;
using ApplicationCore.Factories;
using ApplicationCore.Logging;
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

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfigValidator, SqlConfigValidator>()
                .AddSingleton<IParameterValidator, SqlParameterValidator>()
                .AddSingleton<IColumnTypeManager, SqlColumnTypesManager>()
                .AddSingleton<IPrimaryKeyManager, SqlPrimaryKeyManager>()
                .AddSingleton<ITableInfoCollectionFactory, SqlTableInfoCollectionFactory>()
                .BuildServiceProvider();
            //var myTemplate = new PrimaryKeyValidationTemplate(new List<string> { "cucu", "lucu", "mucu"}, new List<string> { "pucu", "cucu"},
            //    "marko","rusz","ize");

            //string pagecontent = new GetColumnTypes("cucu", "mucu", "lucu", new List<string>()).TransformText();
            //Console.WriteLine(pagecontent);
            //Console.ReadKey();


            var tableInfoCollectionFactory = serviceProvider.GetService<ITableInfoCollectionFactory>();
            var validTables = tableInfoCollectionFactory.CreateTableListFromConfig(databasesConfig); 


            Console.ReadKey();
        }
    }
}