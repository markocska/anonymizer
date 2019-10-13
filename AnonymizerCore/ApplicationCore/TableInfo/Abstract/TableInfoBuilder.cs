using Microsoft.Extensions.Logging;
using Scrambler.Config;
using Scrambler.DatabaseServices.ColumnTypes;
using Scrambler.DatabaseServices.PrimaryKeys;
using Scrambler.TableInfo.Common;
using Scrambler.TableInfo.Exceptions;
using Scrambler.TableInfo.Interfaces;
using Scrambler.Validators;
using Scrambler.Validators.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrambler.TableInfo.Abstract
{
    public abstract class TableInfoBuilder : ITableInfoBuilder
    {
        protected ILogger _logger;
        private IConfigValidator _configValidator;
        private IWhereConditionValidator _whereConditionValidator;
        private IColumnTypeManager _columnTypeManager;
        private ILinkedServerValidator _linkedServerValidator;
        private IPrimaryKeyManager _primaryKeyManager;
        public TableConfig TableConfig { get; private set; }
        public DatabaseConfig DatabaseConfig { get; private set; }

        public TableInfoBuilder(DatabaseConfig dbConfig, TableConfig tableConfig, IConfigValidator configValidator, IWhereConditionValidator whereConditionValidator,
            ILinkedServerValidator linkedServerValidator,IColumnTypeManager columnTypeManager, IPrimaryKeyManager primaryKeyManager, ILogger logger)
        {
            TableConfig = tableConfig;
            DatabaseConfig = dbConfig;
            _configValidator = configValidator;
            _whereConditionValidator = whereConditionValidator;
            _columnTypeManager = columnTypeManager;
            _linkedServerValidator = linkedServerValidator;
            _primaryKeyManager = primaryKeyManager;
            _logger = logger;
        }

        public ITableInfo Build()
        {
            CheckInputParams();

            var tableInfo = CreateTableInfoBase();

            tableInfo.SoloScrambledColumnsAndTypes = GetScrambledColumnTypes(tableInfo);

            tableInfo.ConstantColumnsAndTypesAndValues = GetConstantColumnTypesAndValues(tableInfo);

            tableInfo.PairedColumnsInside = ParseMappedColumnsInside(tableInfo);

            tableInfo.MappedTablesOutside = ParseMappedTablesOutsideFromConfig();

            tableInfo.PrimaryKeysAndTypes = GetPrimaryKeysAndTypes(tableInfo);

            tableInfo.WhereClause = TableConfig.Where;

            return tableInfo;
        }

        private Dictionary<string, string> GetPrimaryKeysAndTypes(TableInfo tableInfo)
        {
            try
            {
                var primaryKeysAndTypes = new Dictionary<string, string>();
                var primaryKeys = _primaryKeyManager.GetPrimaryKeys(DatabaseConfig.ConnectionString, TableConfig.FullTableName);
                primaryKeysAndTypes = _columnTypeManager.GetColumnNamesAndTypes(tableInfo, primaryKeys);

                return primaryKeysAndTypes;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error happened while trying to get primary keys and their types {ex.Message}", ex);
                throw new TableInfoException(TableConfig.FullTableName, DatabaseConfig.ConnectionString, "Error while creating table");
            }
        }

        private List<ColumnAndTypeAndValue> GetConstantColumnTypesAndValues(TableInfo tableInfo)
        {
            Dictionary<string, string> constantColumnsAndTypes;
            try
            {
                constantColumnsAndTypes = _columnTypeManager.GetColumnNamesAndTypes(tableInfo, TableConfig.ConstantColumns.Select(c => c.Name).ToList());
            }
            catch (ColumnTypesException ex)
            {
                _logger.LogError($"Error while getting constant column types for table {TableConfig.FullTableName}. " +
                    $"Connection string: {DatabaseConfig.ConnectionString}. Error message: {ex.Message}. ", ex);
                throw new TableInfoException(TableConfig.FullTableName, DatabaseConfig.ConnectionString, "Error while creating the table.");
            }

            var constantColumnsAndValues = new List<ColumnAndTypeAndValue>();
            foreach (var column in TableConfig.ConstantColumns)
            {
                constantColumnsAndValues.Add(new ColumnAndTypeAndValue { Name = column.Name, Type = constantColumnsAndTypes[column.Name], Value = column.Value });
            }

            return constantColumnsAndValues;
        }

        private TableInfo CreateTableInfoBase()
        {
            TableConfig = NormalizeTableConfigParameters(TableConfig);
            var dbName = ParseDataSource(DatabaseConfig.ConnectionString);

            return  
                new TableInfo
                {
                    DbConnectionString = DatabaseConfig.ConnectionString,
                    DbName = dbName,
                    SchemaName = ParseSchemaAndTableName(TableConfig.FullTableName).schemaName,
                    TableName = ParseSchemaAndTableName(TableConfig.FullTableName).tableName,
                    WhereClause = TableConfig.Where
                };
        }

        private Dictionary<string, string> GetScrambledColumnTypes(TableInfo tableInfo)
        {
            Dictionary<string, string> scrambledColumns;
            try
            {
                scrambledColumns = _columnTypeManager.GetColumnNamesAndTypes(tableInfo, TableConfig.ScrambledColumns.Select(c => c.Name).ToList());
            }
            catch (ColumnTypesException ex)
            {
                _logger.LogError($"Error while getting scrambled column types for table {TableConfig.FullTableName}. " +
                    $"Connection string: {DatabaseConfig.ConnectionString}. Error message: {ex.Message}. ", ex);
                throw new TableInfoException(TableConfig.FullTableName, DatabaseConfig.ConnectionString, "Error while creating the table.");
            }

            return scrambledColumns;
        }

        protected abstract string ParseDataSource(string connectionString);

        protected abstract (string schemaName, string tableName) ParseSchemaAndTableName(string schemaAndTableName);

        protected abstract TableConfig NormalizeTableConfigParameters(TableConfig tableConfig);


        private List<MappedTable> ParseMappedTablesOutsideFromConfig()
        {
            var mappedTablesOutside = new List<MappedTable>();
            for (int i = 0; i < TableConfig.PairedColumnsOutsideTable.Count; i++)
            {
                var mappedTableOutsideConfig = TableConfig.PairedColumnsOutsideTable[i];
                var mappedTable = new MappedTable
                {
                    // columns that will have to same value in the source and dest. table 
                    SourceDestPairedColumnsOutside = mappedTableOutsideConfig.ColumnMapping.Select(m => new ColumnPair(m[0], m[1])).ToList()
                };

                var columnMapping = new List<MappedColumnPair>();
                for (int j = 0; j < mappedTableOutsideConfig.SourceDestMapping.Count; j++)
                {
                    var columnMappingConfig = mappedTableOutsideConfig.SourceDestMapping[j];
                    if (j == 0)
                    {
                        columnMapping.Add(new MappedColumnPair
                        {
                            SourceConnectionString = DatabaseConfig.ConnectionString,
                            SourceTableNameWithSchema = TableConfig.FullTableName,
                            DestinationConnectionString = columnMappingConfig.DestinationConnectionString,
                            DestinationTableNameWithSchema = columnMappingConfig.DestinationFullTableName,
                            DestinationInstance = columnMappingConfig.DestinationLinkedInstance,
                            MappedColumns = columnMappingConfig.ForeignKeyMapping.Select(m => new ColumnPair(m[0], m[1])).ToList()
                        });
                    }
                    else
                    {
                        var previousColumnMappingConfig = mappedTableOutsideConfig.SourceDestMapping[j - 1];
                        columnMapping.Add(new MappedColumnPair
                        {
                            SourceConnectionString = previousColumnMappingConfig.DestinationConnectionString,
                            SourceTableNameWithSchema = previousColumnMappingConfig.DestinationFullTableName,
                            SourceInstance = previousColumnMappingConfig.DestinationLinkedInstance,
                            DestinationConnectionString = columnMappingConfig.DestinationConnectionString,
                            DestinationTableNameWithSchema = columnMappingConfig.DestinationFullTableName,
                            DestinationInstance = columnMappingConfig.DestinationLinkedInstance,
                            MappedColumns = columnMappingConfig.ForeignKeyMapping.Select(m => new ColumnPair(m[0], m[1])).ToList()
                        });
                    }
                }

                mappedTable.MappedColumnPairsOutside = columnMapping;

                mappedTablesOutside.Add(mappedTable);

            }

            return mappedTablesOutside;
        }

        private List<Dictionary<string, string>> ParseMappedColumnsInside(ITableInfo tableInfo)
        {
            var pairedColumnsWithTypesList = new List<Dictionary<string, string>>();
            foreach (var pairedColumns in TableConfig.PairedColumnsInsideTable)
            {
                var pairedColumnsWithTypes = new Dictionary<string, string>();

                foreach (var column in pairedColumns)
                {
                    pairedColumnsWithTypes.Add(column, tableInfo.SoloScrambledColumnsAndTypes[column]);
                    tableInfo.SoloScrambledColumnsAndTypes.Remove(column);
                }
                pairedColumnsWithTypesList.Add(pairedColumnsWithTypes);
            }

            return pairedColumnsWithTypesList;
        }

        private void CheckInputParams()
        {
            if (!_configValidator.IsDbConfigValid(DatabaseConfig))
            {
                throw new TableInfoException(TableConfig.FullTableName, DatabaseConfig.ConnectionString,
                    $"Error while creating the TableInfo object.");
            }

            if (!_configValidator.IsTableConfigValid(DatabaseConfig, TableConfig))
            {
                throw new TableInfoException(TableConfig.FullTableName, DatabaseConfig.ConnectionString,
                    $"Error while creating the TableInfo object.");
            }

            if (!_whereConditionValidator.IsWhereConditionValid(DatabaseConfig.ConnectionString, TableConfig))
            {
                throw new TableInfoException(TableConfig.FullTableName, DatabaseConfig.ConnectionString,
                    $"Error while creating the TableInfo object.");
            }

            if (!_linkedServerValidator.AreLinkedServerParamsValid(DatabaseConfig.ConnectionString, TableConfig))
            {
                throw new TableInfoException(TableConfig.FullTableName, DatabaseConfig.ConnectionString,
                    $"Error while creating the TableInfo object.");
            }
        }

        protected class TableInfo : ITableInfo
        {
            public string DbConnectionString { get; set; }
            public string DbName { get; set; }
            public string SchemaName { get; set; }
            public string TableName { get; set; }
            public Dictionary<string, string> PrimaryKeysAndTypes { get; set; }
            public Dictionary<string, string> SoloScrambledColumnsAndTypes { get; set; }
            public List<ColumnAndTypeAndValue> ConstantColumnsAndTypesAndValues { get; set; }
            public string WhereClause { get; set; }
            public string FullTableName => $"{DbName}.{SchemaName}.{TableName}";
            public List<Dictionary<string, string>> PairedColumnsInside { get; set; }
            public List<MappedTable> MappedTablesOutside { get; set; }
        }
    }
}
