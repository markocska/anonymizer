using Scrambler.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Scrambler.DatabaseServices.Scrambling;
using Scrambler.Factories;
using Scrambler.Validators.ConfigValidators;
using Scrambler.Validators.ParameterValidators;
using Scrambler.DatabaseServices.ColumnTypes;
using Scrambler.DatabaseServices.PrimaryKeys;

namespace Scrambler
{
    public abstract class ScramblingService<TConfigValidator, TParameterValidator, TColumnTypeManager, TPrimaryKeyManager, TTableInfoCollectionFactory>
        : IScramblingService
        where TConfigValidator : class, IConfigValidator
        where TParameterValidator : class, IParameterValidator
        where TColumnTypeManager : class, IColumnTypeManager
        where TPrimaryKeyManager : class, IPrimaryKeyManager
        where TTableInfoCollectionFactory : class, ITableInfoCollectionFactory
    {
        private readonly ServiceProvider _serviceProvider;

        public ScramblingService()
        {
            var serviceProvider = new ServiceCollection()
                 .AddScoped<IConfigValidator, TConfigValidator>()
                 .AddScoped<IParameterValidator, TParameterValidator>()
                 .AddScoped<IColumnTypeManager, TColumnTypeManager>()
                 .AddScoped<IPrimaryKeyManager, TPrimaryKeyManager>()
                 .AddScoped<ITableInfoCollectionFactory, TTableInfoCollectionFactory>()
                 .BuildServiceProvider();

            _serviceProvider = serviceProvider;
        }

        public void ScrambleFromConfigStr(string configStr)
        {
            var config = File.ReadAllText(configStr);
            var databasesConfig = JsonConvert.DeserializeObject<DatabasesConfig>(config);


            if (databasesConfig == null)
            {
                throw new ArgumentNullException("The DatabaseConfiguration parameter is null.");
            }

            var dbs = databasesConfig.Databases;
            if (dbs == null)
            {
                return;
            }

            var validTables = _serviceProvider.GetRequiredService<ITableInfoCollectionFactory>().CreateTableListFromConfig(databasesConfig);

            var tableScramblingService = _serviceProvider.GetRequiredService<ITableScramblingService>();
            foreach (var table in validTables)
            {
                var scramblingScript = tableScramblingService.GenerateScramblingScript(table);
                Console.WriteLine(scramblingScript);
            }

        }

        public void ScrambleFromConfigPath(string configPath)
        {

        }
    }
}
