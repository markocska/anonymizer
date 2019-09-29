using Microsoft.Extensions.Logging;
using Scrambler.Config;
using Scrambler.DatabaseServices.ColumnTypes;
using Scrambler.DatabaseServices.PrimaryKeys;
using Scrambler.TableInfo;
using Scrambler.TableInfo.Abstract;
using Scrambler.TableInfo.Interfaces;
using Scrambler.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.Factories
{
    public abstract class TableInfoCollectionFactory : ITableInfoCollectionFactory
    {
        protected readonly ILogger _logger;

        private readonly IConfigValidator _configValidator;
        private readonly IParameterValidator _parameterValidator;
        private readonly IColumnTypeManager _columnTypeManager;
        private readonly IPrimaryKeyManager _primaryKeyManager;

        public TableInfoCollectionFactory(IConfigValidator configValidator, IParameterValidator parameterValidator, IColumnTypeManager columnTypeManager,
            IPrimaryKeyManager primaryKeyManager, ILogger<TableInfoCollectionFactory> logger)
        {
            _logger = logger;
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

            foreach (var dbConfig in databasesConfig.Databases)
            {
                if (!_configValidator.IsDbConfigValid(dbConfig))
                {
                    databasesConfig.Databases.Remove(dbConfig);
                }

                foreach (var tableConfig in dbConfig.Tables)
                {
                    if (!_configValidator.IsTableConfigValid(dbConfig, tableConfig) ||
                        !_parameterValidator.AreTheParamsValid(dbConfig.ConnectionString, tableConfig))
                    {
                            dbConfig.Tables.Remove(tableConfig);
                    }
                }
            }

            var tableInfos = new List<ITableInfo>();
            foreach (var dbConfig in databasesConfig.Databases)
            {
                foreach (var tableConfig in dbConfig.Tables)
                {
                    try
                    {
                        var tableInfo = CreateTableInfo(dbConfig, tableConfig, _configValidator, _columnTypeManager, _primaryKeyManager);
                        tableInfos.Add(tableInfo);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"An error happened while getting table information for table {tableConfig.FullTableName}. " +
                            $"DbConnectionString: {dbConfig.ConnectionString}. Error message: {ex.Message}");
                    }
                }
            }

            return tableInfos;

        }

        protected abstract ITableInfo CreateTableInfo(DatabaseConfig dbConfig, TableConfig tableConfig, IConfigValidator configValidator, 
            IColumnTypeManager columnTypeManager, IPrimaryKeyManager primaryKeyManager);
    }
}
