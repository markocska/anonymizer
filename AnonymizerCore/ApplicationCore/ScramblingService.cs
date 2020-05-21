using Scrambler.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Scrambler.DatabaseServices.Scrambling;
using Scrambler.Factories;
using Scrambler.Validators;
using Scrambler.DatabaseServices.ColumnTypes;
using Scrambler.DatabaseServices.PrimaryKeys;
using Scrambler.Utilities;
using Microsoft.Extensions.Logging;
using Scrambler.Validators.Interfaces;
using Serilog;
using Serilog.Core;

namespace Scrambler
{
    public abstract class ScramblingService<TConfigValidator, TParameterValidator, TColumnTypeManager, TPrimaryKeyManager, TTableInfoCollectionFactory,
        TTableScramblingService,TWhereConditionValidator ,TLinkedServerValidator ,TQueryHelper>
        : IScramblingService
        where TConfigValidator : class, IConfigValidator
        where TParameterValidator : class, IParameterValidator
        where TColumnTypeManager : class, IColumnTypeManager
        where TPrimaryKeyManager : class, IPrimaryKeyManager
        where TTableInfoCollectionFactory : class, ITableInfoCollectionFactory
        where TTableScramblingService : class, ITableScramblingService
        where TWhereConditionValidator : class, IWhereConditionValidator
        where TLinkedServerValidator : class, ILinkedServerValidator
        where TQueryHelper : class, IQueryHelper
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly IQueryHelper _queryHelper;

        public ScramblingService(IQueryHelper queryHelper, Logger logger)
        {
            var serviceProvider = new ServiceCollection()
                 .AddScoped<IConfigValidator, TConfigValidator>()
                 .AddScoped<IParameterValidator, TParameterValidator>()
                 .AddScoped<IColumnTypeManager, TColumnTypeManager>()
                 .AddScoped<IPrimaryKeyManager, TPrimaryKeyManager>()
                 .AddScoped<ITableInfoCollectionFactory, TTableInfoCollectionFactory>()
                 .AddScoped<ITableScramblingService, TTableScramblingService>()
                 .AddScoped<IWhereConditionValidator, TWhereConditionValidator>()
                 .AddScoped<ILinkedServerValidator, TLinkedServerValidator>()
                 .AddScoped<IQueryHelper, TQueryHelper>()
                 .BuildServiceProvider();

            Log.Logger = logger;
            _serviceProvider = serviceProvider;
            _queryHelper = queryHelper;
        }

        public void ScrambleFromConfigStr(string configStr)
        {
            var databasesConfig = JsonConvert.DeserializeObject<DatabasesConfig>(configStr);


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
                tableScramblingService.ScrambleTable(table, scramblingScript);               
            }


        }

        public void ScrambleFromConfigPath(string configPath)
        {

        }
    }
}
