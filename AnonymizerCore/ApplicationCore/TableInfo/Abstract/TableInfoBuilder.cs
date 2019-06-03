using ApplicationCore.Config;
using ApplicationCore.DatabaseServices.ColumnTypes;
using ApplicationCore.DatabaseServices.PrimaryKeys;
using ApplicationCore.TableInfo.Common;
using ApplicationCore.TableInfo.Exceptions;
using ApplicationCore.TableInfo.Interfaces;
using ApplicationCore.Utilities;
using ApplicationCore.Validators.ConfigValidators;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.TableInfo.Abstract
{
    public abstract class TableInfoBuilder : ITableInfoBuilder
    {
        protected ILogger _logger;
        private IConfigValidator _configValidator;
        private IColumnTypeManager _columnTypeManager;
        private IPrimaryKeyManager _primaryKeyManager;
        public TableConfig TableConfig { get; private set; }
        public DatabaseConfig DatabaseConfig { get; private set; }

        public TableInfoBuilder(DatabaseConfig dbConfig, TableConfig tableConfig, IConfigValidator configValidator, IColumnTypeManager columnTypeManager,
            IPrimaryKeyManager primaryKeyManager)
        {
            TableConfig = tableConfig;
            DatabaseConfig = dbConfig;
            _configValidator = configValidator;
            _columnTypeManager = columnTypeManager;
            _primaryKeyManager = primaryKeyManager;
            _logger = Serilog.Log.Logger.ForContext<TableInfoBuilder>();
        }

        public ITableInfo Build()
        {

            if (!_configValidator.IsDbConfigValid(DatabaseConfig))
            {
                throw new TableInfoException(TableConfig.NameWithSchema, DatabaseConfig.ConnectionString,
                    $"Error while creating the TableInfo object.");
            }

            if (!_configValidator.IsTableConfigValid(DatabaseConfig, TableConfig))
            {
                throw new TableInfoException(TableConfig.NameWithSchema, DatabaseConfig.ConnectionString,
                    $"Error while creating the TableInfo object.");
            }

            TableConfig = NormalizeTableConfigParameters(TableConfig);
            var dbName = ParseDataSource(DatabaseConfig.ConnectionString);

            var tableInfo =
                new TableInfo
                {
                    DbConnectionString = DatabaseConfig.ConnectionString,
                    DbName = dbName,
                    SchemaName = ParseSchemaAndTableName(TableConfig.NameWithSchema).schemaName,
                    TableName = ParseSchemaAndTableName(TableConfig.NameWithSchema).tableName,
                    WhereClause = ""
                };


            Dictionary<string, string> scrambledColumns;
            try
            {
                scrambledColumns = _columnTypeManager.GetColumnNamesAndTypes(tableInfo, TableConfig.ScrambledColumns.Select(c => c.Name).ToList());
            }
            catch(ColumnTypesException ex)
            {
                _logger.Error($"Error while getting scrambled column types for table {TableConfig.NameWithSchema}. " +
                    $"Connection string: {DatabaseConfig.ConnectionString}. Error message: {ex.Message}. ", ex);
                throw new TableInfoException(TableConfig.NameWithSchema, DatabaseConfig.ConnectionString, "Error while creating the table.");
            }

            tableInfo.ScrambledColumnsAndTypes = scrambledColumns;

            Dictionary<string, string> constantColumnsAndTypes;
            try
            {
                constantColumnsAndTypes = _columnTypeManager.GetColumnNamesAndTypes(tableInfo, TableConfig.ConstantColumns.Select(c => c.Name).ToList());
            }
            catch (ColumnTypesException ex)
            {
                _logger.Error($"Error while getting constant column types for table {TableConfig.NameWithSchema}. " +
                    $"Connection string: {DatabaseConfig.ConnectionString}. Error message: {ex.Message}. ", ex);
                throw new TableInfoException(TableConfig.NameWithSchema, DatabaseConfig.ConnectionString, "Error while creating the table.");
            }

            var constantColumnsAndValues = new List<ColumnAndTypeAndValue>();
            foreach (var column in TableConfig.ConstantColumns)
            {
                constantColumnsAndValues.Add(new ColumnAndTypeAndValue { Name = column.Name, Type = constantColumnsAndTypes[column.Name], Value = column.Value });
            }

            tableInfo.ConstantColumnsAndTypesAndValues = constantColumnsAndValues;

            tableInfo.PairedColumnsInside = TableConfig.PairedColumnsInsideTable;

            tableInfo.MappedTablesOutside = ParseMappedTablesOutsideFromConfig();

            var primaryKeysAndTypes = new Dictionary<string, string>();
            try
            {
                var primaryKeys = _primaryKeyManager.GetPrimaryKeys(DatabaseConfig.ConnectionString, TableConfig.NameWithSchema);
                primaryKeysAndTypes = _columnTypeManager.GetColumnNamesAndTypes(tableInfo, primaryKeys);
                tableInfo.PrimaryKeysAndTypes = primaryKeysAndTypes;
            }
            catch (Exception ex)
            {
                _logger.Error($"An error happened while trying to get primary keys and their types {ex.Message}", ex);
                throw new TableInfoException(TableConfig.NameWithSchema, DatabaseConfig.ConnectionString, "Error while creating table");
            }


            return tableInfo;
        }

        protected abstract string ParseDataSource(string connectionString);

        protected abstract (string schemaName, string tableName) ParseSchemaAndTableName(string schemaAndTableName);

        protected abstract TableConfig NormalizeTableConfigParameters(TableConfig tableConfig);

        //private string RemoveParenthesis(string column)
        //{
        //    var columnNameWithoutParenthesis = column.Trim(' ');
        //    if (columnNameWithoutParenthesis.StartsWith("[") && columnNameWithoutParenthesis.EndsWith("]"))
        //    {
        //        columnNameWithoutParenthesis = columnNameWithoutParenthesis.Remove(0, 1);

        //        var length = columnNameWithoutParenthesis.Length;
        //        columnNameWithoutParenthesis = columnNameWithoutParenthesis.Remove(length - 1, 1);
        //    }

        //    return columnNameWithoutParenthesis;
        //}

        private List<MappedTable> ParseMappedTablesOutsideFromConfig()
        {
            var mappedTablesOutside = new List<MappedTable>();
            for (int i = 0; i < TableConfig.PairedColumnsOutsideTable.Count; i++)
            {
                var mappedTableOutsideConfig = TableConfig.PairedColumnsOutsideTable[i];
                var mappedTable = new MappedTable();

                mappedTable.SourceDestPairedColumnsOutside = mappedTableOutsideConfig.ColumnMapping.Select(m => new ColumnPair(m[0], m[1])).ToList();

                var columnMapping = new List<MappedColumnPair>();
                for (int j = 0; j < mappedTableOutsideConfig.SourceDestMapping.Count; j++)
                {
                    var columnMappingConfig = mappedTableOutsideConfig.SourceDestMapping[j];
                    if (j == 0)
                    {
                        columnMapping.Add(new MappedColumnPair
                        {
                            SourceConnectionString = DatabaseConfig.ConnectionString,
                            SourceTableNameWithSchema = TableConfig.NameWithSchema,
                            DestinationConnectionString = columnMappingConfig.DestinationConnectionString,
                            DestinationTableNameWithSchema = columnMappingConfig.DestinationTableNameWithSchema,
                            MappedColumns = columnMappingConfig.ForeignKeyMapping.Select(m => new ColumnPair(m[0], m[1])).ToList()
                        });
                    }
                    else
                    {
                        var previousColumnMappingConfig = mappedTableOutsideConfig.SourceDestMapping[j - 1];
                        columnMapping.Add(new MappedColumnPair
                        {
                            SourceConnectionString = previousColumnMappingConfig.DestinationConnectionString,
                            SourceTableNameWithSchema = previousColumnMappingConfig.DestinationTableNameWithSchema,
                            DestinationConnectionString = columnMappingConfig.DestinationConnectionString,
                            DestinationTableNameWithSchema = columnMappingConfig.DestinationTableNameWithSchema,
                            MappedColumns = columnMappingConfig.ForeignKeyMapping.Select(m => new ColumnPair(m[0], m[1])).ToList()
                        });
                    }
                }

                mappedTable.MappedColumnPairsOutside = columnMapping;

                mappedTablesOutside.Add(mappedTable);

            }

            return mappedTablesOutside;
        }

        protected class TableInfo : ITableInfo
        {
            public string DbConnectionString { get; set; }
            public string DbName { get; set; }
            public string SchemaName { get; set; }
            public string TableName { get; set; }
            public Dictionary<string, string> PrimaryKeysAndTypes { get; set; }
            public Dictionary<string, string> ScrambledColumnsAndTypes { get; set; }
            public List<ColumnAndTypeAndValue> ConstantColumnsAndTypesAndValues { get; set; }
            public string WhereClause { get; set; }
            public string FullTableName => $"{DbName}.{SchemaName}.{TableName}";
            public List<List<string>> PairedColumnsInside { get; set; }
            public List<MappedTable> MappedTablesOutside { get; set; }
        }
    }
}
