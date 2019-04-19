using ApplicationCore.Config;
using ApplicationCore.TableInfo;
using ApplicationCore.Validators;
using ApplicationCore.Validators.ConfigValidators;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Factories
{
    public class TableInfoCollectionFactory
    {
        private readonly ILogger _logger;

        private readonly IConfigValidator _configValidator;

        public TableInfoCollectionFactory(IConfigValidator configValidator)
        {
            _logger = Serilog.Log.ForContext(typeof(TableInfoCollectionFactory));
            _configValidator = configValidator;
        }

        //public List<ITableInfo> CreateTableListFromConfig(DatabasesConfig config)
        //{
        //    var tablesToAnonymize = new List<ITableInfo>();
        //    if (config == null)
        //    {
        //        throw new ArgumentNullException("The DatabaseConfigurationSection input parameter is null.");
        //    }

        //    var dbs = config.Databases;
        //    if (dbs == null)
        //    {
        //        return tablesToAnonymize;
        //    }

        //    foreach (var dbConfig in config.Databases)
        //    {
        //        if (!_configValidator.IsDbConfigValid(dbConfig))
        //        {
        //            continue;
        //        }

        //        foreach (var tableConfig in dbConfig.Tables)
        //        {

        //        }
        //    }
        //}
    }
}
