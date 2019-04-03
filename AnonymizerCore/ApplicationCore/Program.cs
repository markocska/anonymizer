using ApplicationCore.Config;
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
            var config = File.ReadAllText(".\\tablesConfig.json");
            var tablesConfig = JsonConvert.DeserializeObject<DatabasesConfig>(config);

            Console.ReadKey();

        }
    }
}