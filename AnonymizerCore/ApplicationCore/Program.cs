using ApplicationCore.Config;
using ApplicationCore.DatabaseServices.ColumnTypes;
using ApplicationCore.DatabaseServices.PrimaryKeys;
using ApplicationCore.Logging;
using ApplicationCore.SqlScripts.SqlServer;
using ApplicationCore.TableInfo;
using ApplicationCore.TableInfo.Interfaces;
using ApplicationCore.Validators.ConfigValidators;
using ApplicationCore.Validators.ParameterValidators;
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

            //var serviceProvider = new ServiceCollection()
            //    .AddSingleton<IConfigValidator, SqlConfigValidator>()
            //    .BuildServiceProvider();
            //var myTemplate = new PrimaryKeyValidationTemplate(new List<string> { "cucu", "lucu", "mucu"}, new List<string> { "pucu", "cucu"},
            //    "marko","rusz","ize");

            //string pagecontent = new GetColumnTypes("cucu", "mucu", "lucu", new List<string>()).TransformText();
            //Console.WriteLine(pagecontent);
            //Console.ReadKey();

            var validTableConfigs = new List<(DatabaseConfig databaseConfig, TableConfig tableConfig)>();
            //Console.WriteLine(pageContent);
            var parameterValidator = new SqlParameterValidator();
            var sqlConfigValidator = new SqlConfigValidator();
            foreach (var dbConfig in databasesConfig.Databases)
            {
                if (!sqlConfigValidator.IsDbConfigValid(dbConfig))
                {
                    continue;
                }

                foreach (var tableConfig in dbConfig.Tables)
                {
                    if (sqlConfigValidator.IsTableConfigValid(dbConfig, tableConfig))
                    {
                        if (parameterValidator.AreTheParamsValid(dbConfig.ConnectionString, tableConfig))
                        {
                            validTableConfigs.Add((databaseConfig: dbConfig, tableConfig: tableConfig));
                        }
                    }
                }
            }

            var tableInfos = new List<ITableInfo>();
            var columnTypeManager = new SqlColumnTypesManager();
            var primaryKeyManager = new SqlPrimaryKeyManager();
            foreach (var (dbConfig,tableConfig) in validTableConfigs)
            {
                var tableInfoBuilder = new SqlTableInfoBuilder(dbConfig, tableConfig, sqlConfigValidator, columnTypeManager, primaryKeyManager);
                tableInfos.Add(tableInfoBuilder.Build());
            }


            Console.ReadKey();
        }
    }
}