using ApplicationCore.Config;
using ApplicationCore.Logging;
using ApplicationCore.Validators.ConfigValidators;
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

            //string pageContent = myTemplate.TransformText();

            var validTableConfigs = new List<(string connectionString, TableConfig tableConfig)>();

            //Console.WriteLine(pageContent);
            var sqlConfigValidator = new SqlConfigValidator();
            foreach (var dbConfig in databasesConfig.Databases)
            {
                if (!sqlConfigValidator.IsDbConfigValid(dbConfig))
                {
                    continue;
                }

                foreach (var tableConfig in dbConfig.Tables)
                {
                    if (!sqlConfigValidator.IsTableConfigValid(dbConfig, tableConfig))
                    {
                        continue;
                    }
                    else
                    {
                        validTableConfigs.Add((connectionString: dbConfig.ConnectionString, tableConfig: tableConfig));
                    }
                }
            }

            //var primKeys = new SqlParameterValidator();

            //try
            //{
            //    bool result = true;
            //    foreach (var database in databasesConfig.Databases)
            //    {
            //        foreach (var tableConfig in database.Tables)
            //        {
            //            result = primKeys.AreTheParamsValid(database.ConnectionString, tableConfig);
            //            Console.WriteLine(result);
            //        }
            //    }
            //    Console.ReadKey();
            //}
            //catch (Exception ex)
            //{
            //    Console.ReadKey();
            //}


            Console.ReadKey();
        }
    }
}