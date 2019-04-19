using ApplicationCore.Config;
using ApplicationCore.Validators;
using ApplicationCore.Validators.ConfigValidators;
using ApplicationCore.Validators.ParameterValidators;
using ApplicationCore.Validators.ParameterValidators.Templates;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;

namespace ApplicationCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //var config = File.ReadAllText(".\\tablesConfig.json");
            //var tablesConfig = JsonConvert.DeserializeObject<DatabasesConfig>(config);

            //var serviceProvider = new ServiceCollection()
            //    .AddSingleton<IConfigValidator, SqlConfigValidator>()
            //    .BuildServiceProvider();
            //var myTemplate = new PrimaryKeyValidationTemplate(new List<string> { "cucu", "lucu", "mucu"}, new List<string> { "pucu", "cucu"},
            //    "marko","rusz","ize");

            //string pageContent = myTemplate.TransformText();



            //Console.WriteLine(pageContent);

            var primKeys = new SqlParameterValidator();
            var result = primKeys.CheckParams("data source=MARKO-PC\\SQLEXPRESS;initial catalog=People;integrated security=True;", "dbo.[cucuka]",
                new List<string> {"id","anya", "baba" }, new List<string> { "name"});

            Console.WriteLine(result);
            Console.ReadKey();

        }
    }
}