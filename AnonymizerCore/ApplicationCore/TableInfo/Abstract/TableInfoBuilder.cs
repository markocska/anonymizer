using ApplicationCore.Config;
using ApplicationCore.DatabaseServices.ColumnTypes;
using ApplicationCore.TableInfo.Common;
using ApplicationCore.TableInfo.Exceptions;
using ApplicationCore.TableInfo.Interfaces;
using ApplicationCore.Validators;
using ApplicationCore.Validators.ConfigValidators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ApplicationCore.TableInfo.Abstract
{
    public abstract class TableInfoBuilder : ITableInfoBuilder
    {
        private IConfigValidator _configValidator;
        private IColumnTypeManager _columnTypeManager;
        public TableConfig TableConfig { get; }
        public DatabaseConfig DatabaseConfig { get; }

        public TableInfoBuilder(DatabaseConfig dbConfig, TableConfig tableConfig, IConfigValidator configValidator, IColumnTypeManager columnTypeManager)
        {
            TableConfig = tableConfig;
            DatabaseConfig = dbConfig;
            _configValidator = configValidator;
            _columnTypeManager = columnTypeManager;
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

            var scrambledColumns = _columnTypeManager.GetColumnNamesAndTypes(tableInfo, TableConfig.ScrambledColumns.Select(c => c.Name).ToList());

            var constantColumnsAndTypes = _columnTypeManager.GetColumnNamesAndTypes(tableInfo, TableConfig.ConstantColumns.Select(c => c.Name).ToList());

            var constantColumnsAndValues = new List<ColumnAndTypeAndValue>();
            foreach (var column in TableConfig.ConstantColumns)
            {
                constantColumnsAndValues.Add(new ColumnAndTypeAndValue {Name = column.Name, Type = constantColumnsAndTypes[column.Name], Value = column.Value });
            }

            tableInfo.ConstantColumnsAndValues = constantColumnsAndValues;
            tableInfo.ScrambledColumns = scrambledColumns;

            tableInfo.PairedColumnsInside = TableConfig.PairedColumnsInsideTable;

            return tableInfo;
        }

        protected abstract string ParseDataSource(string connectionString);

        protected abstract (string schemaName, string tableName) ParseSchemaAndTableName(string schemaAndTableName);

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

        protected class TableInfo : ITableInfo
        {
            public string DbConnectionString { get; set; }
            public string DbName { get; set; }
            public string SchemaName { get; set; }
            public string TableName { get; set; }
            public Dictionary<string, string> ScrambledColumns { get; set; }
            public List<ColumnAndTypeAndValue> ConstantColumnsAndValues { get; set; }
            public string WhereClause { get; set; }
            public string FullTableName => $"{DbName}.{SchemaName}.{TableName}";
            public List<List<string>> PairedColumnsInside { get; set; }
            public ColumnPair SourceDestPairedColumnsOutside { get; set; }
            public List<MappedColumnPair> mappedColumnPairsOutside { get; set; }
        }
    }
}
