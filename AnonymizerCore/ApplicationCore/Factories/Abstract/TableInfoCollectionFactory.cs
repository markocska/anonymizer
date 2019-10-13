using Microsoft.Extensions.Logging;
using Scrambler.Config;
using Scrambler.DatabaseServices.ColumnTypes;
using Scrambler.DatabaseServices.PrimaryKeys;
using Scrambler.TableInfo.Interfaces;
using Scrambler.Validators;
using Scrambler.Validators.Interfaces;
using System;
using System.Collections.Generic;

namespace Scrambler.Factories
{
    public abstract class TableInfoCollectionFactory : ITableInfoCollectionFactory
    {
        protected readonly ILogger _logger;

        protected readonly IConfigValidator _configValidator;
        protected readonly IParameterValidator _parameterValidator;
        protected readonly IColumnTypeManager _columnTypeManager;
        protected readonly IPrimaryKeyManager _primaryKeyManager;
        protected readonly IWhereConditionValidator _whereConditionValidator;
        protected readonly ILinkedServerValidator _linkedServerValidator;

        public TableInfoCollectionFactory(IConfigValidator configValidator, IParameterValidator parameterValidator, IColumnTypeManager columnTypeManager,
            IPrimaryKeyManager primaryKeyManager, IWhereConditionValidator whereConditionValidator, ILinkedServerValidator linkedServerValidator, ILogger<TableInfoCollectionFactory> logger)
        {
            _logger = logger;
            _configValidator = configValidator;
            _parameterValidator = parameterValidator;
            _columnTypeManager = columnTypeManager;
            _primaryKeyManager = primaryKeyManager;
            _whereConditionValidator = whereConditionValidator;
            _linkedServerValidator = linkedServerValidator;
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

            var dbConfigsToRemove = new List<DatabaseConfig>();
            foreach (var dbConfig in databasesConfig.Databases)
            {
                if (!_configValidator.IsDbConfigValid(dbConfig))
                {
                    dbConfigsToRemove.Add(dbConfig);
                }

                var tableConfigsToRemove = new List<TableConfig>();
                foreach (var tableConfig in dbConfig.Tables)
                {
                    if (!_configValidator.IsTableConfigValid(dbConfig, tableConfig) ||
                        !_parameterValidator.AreTheParamsValid(dbConfig.ConnectionString, tableConfig) ||
                        !_whereConditionValidator.IsWhereConditionValid(dbConfig.ConnectionString, tableConfig) ||
                        !_linkedServerValidator.AreLinkedServerParamsValid(dbConfig.ConnectionString, tableConfig))
                    {
                        tableConfigsToRemove.Add(tableConfig);
                    }
                }
                dbConfig.Tables.RemoveAll(t => tableConfigsToRemove.Contains(t));
            }
            databasesConfig.Databases.RemoveAll(d => dbConfigsToRemove.Contains(d));

            var tableInfos = new List<ITableInfo>();
            foreach (var dbConfig in databasesConfig.Databases)
            {
                foreach (var tableConfig in dbConfig.Tables)
                {
                    try
                    {
                        var tableInfo = CreateTableInfo(dbConfig, tableConfig);
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

        protected abstract ITableInfo CreateTableInfo(DatabaseConfig dbConfig, TableConfig tableConfig);
    }
}
