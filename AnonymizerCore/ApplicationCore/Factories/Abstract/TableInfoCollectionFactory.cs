using ApplicationCore.Config;
using ApplicationCore.DatabaseServices.ColumnTypes;
using ApplicationCore.DatabaseServices.PrimaryKeys;
using ApplicationCore.TableInfo;
using ApplicationCore.TableInfo.Abstract;
using ApplicationCore.TableInfo.Interfaces;
using ApplicationCore.Validators;
using ApplicationCore.Validators.ConfigValidators;
using ApplicationCore.Validators.ParameterValidators;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Factories
{
    public abstract class TableInfoCollectionFactory : ITableInfoCollectionFactory
    {
        private readonly ILogger _logger;

        private readonly IConfigValidator _configValidator;
        private readonly IParameterValidator _parameterValidator;
        private readonly IColumnTypeManager _columnTypeManager;
        private readonly IPrimaryKeyManager _primaryKeyManager;

        public TableInfoCollectionFactory(IConfigValidator configValidator, IParameterValidator parameterValidator, IColumnTypeManager columnTypeManager,
            IPrimaryKeyManager primaryKeyManager)
        {
            _logger = Serilog.Log.ForContext(typeof(TableInfoCollectionFactory));
            _configValidator = configValidator;
            _parameterValidator = parameterValidator;
            _columnTypeManager = columnTypeManager;
            _primaryKeyManager = primaryKeyManager;
        }

        public List<ITableInfo> CreateTableListFromConfig(DatabasesConfig databasesConfig)
        {
            if (databasesConfig == null)
            {
                throw new ArgumentNullException("The DatabaseConfigurationSection input parameter is null.");
            }

            var dbs = databasesConfig.Databases;
            if (dbs == null)
            {
                return new List<ITableInfo>();
            }

            var validTableConfigs = new List<(DatabaseConfig databaseConfig, TableConfig tableConfig)>();
            foreach (var dbConfig in databasesConfig.Databases)
            {
                if (!_configValidator.IsDbConfigValid(dbConfig))
                {
                    continue;
                }

                foreach (var tableConfig in dbConfig.Tables)
                {
                    if (_configValidator.IsTableConfigValid(dbConfig, tableConfig))
                    {
                        if (_parameterValidator.AreTheParamsValid(dbConfig.ConnectionString, tableConfig))
                        {
                            validTableConfigs.Add((databaseConfig: dbConfig, tableConfig: tableConfig));
                        }
                    }
                }
            }

            var tableInfos = new List<ITableInfo>();
            foreach (var (dbConfig, tableConfig) in validTableConfigs)
            {
                try
                {
                    var tableInfo = CreateTableInfo(dbConfig, tableConfig, _configValidator, _columnTypeManager, _primaryKeyManager);
                    tableInfos.Add(tableInfo);
                }
                catch (Exception ex)
                {
                    _logger.Error($"An error happened while getting table information for table {tableConfig.NameWithSchema}. " +
                        $"DbConnectionString: {dbConfig.ConnectionString}. Error message: {ex.Message}");
                }
            }

            return tableInfos;

        }

        protected abstract ITableInfo CreateTableInfo(DatabaseConfig dbConfig, TableConfig tableConfig, IConfigValidator configValidator, 
            IColumnTypeManager columnTypeManager, IPrimaryKeyManager primaryKeyManager);
    }
}
