using ApplicationCore.Config;
using ApplicationCore.TableInfo.Exceptions;
using ApplicationCore.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.TableInfo
{
    public class TableInfoBuilder
    {
        //private IConfigValidator _configValidator;
        //public TableConfig TableConfig { get; }
        //public DatabaseConfig DatabaseConfig { get;}
    
        //public TableInfoBuilder(DatabaseConfig dbConfig, TableConfig tableConfig, IConfigValidator configValidator)
        //{
        //    TableConfig = tableConfig;
        //    DatabaseConfig = dbConfig;
        //    _configValidator = configValidator;
        //}

        //public ITableInfo Build()
        //{
            
        //    if (!_configValidator.IsDbConfigValid(DatabaseConfig))
        //    {
        //        throw new TableInfoException(TableConfig.NameWithSchema, DatabaseConfig.ConnectionString,
        //            $"Error while creating the TableInfo object.");
        //    }

        //    if (!_configValidator.IsTableConfigValid(DatabaseConfig, TableConfig))
        //    {
        //        throw new TableInfoException(TableConfig.NameWithSchema, DatabaseConfig.ConnectionString,
        //            $"Error while creating the TableInfo object.");
        //    }

        //    var dbName = ParseDataSource(DatabaseConfig.ConnectionString);

        //    var tableInfo =
        //        new TableInfo
        //        {
        //            DbConnectionString = DatabaseConfig.ConnectionString,
        //            DbName = RemoveParenthesis(dbName),
        //            SchemaName = RemoveParenthesis(ParseSchemaAndTableName(TableConfig.NameWithSchema).schemaName),
        //            TableName = RemoveParenthesis(ParseSchemaAndTableName(TableConfig.NameWithSchema).tableName),
        //            WhereClause = ""
        //        };

        //    var constantColumnsAndValues = new Dictionary<string, string>();
        //    var constantColumns = new List<string>();
        //    var scrambledColumns = new List<string>();

        //    for (int i = 0; i < TableConfig.ScrambledColumns.Count; i++)
        //    {
        //        var column = TableConfig.ScrambledColumns[i];

        //        scrambledColumns.Add(RemoveParenthesis(column.Name));
        //    }

        //    for (int i = 0; i < TableConfig.ConstantColumns.Count; i++)
        //    {
        //        var column = TableConfig.ConstantColumns[i];

        //        constantColumns.Add(RemoveParenthesis(column.Name));
        //        constantColumnsAndValues.Add(RemoveParenthesis(column.Name), column.Value);
        //    }

        //    var inputParameterValidationResult = SqlParameterValidator.AreInputParamsValid(tableInfo, scrambledColumns, constantColumns);
        //    if (!inputParameterValidationResult.IsValid)
        //    {
        //        throw new TableInfoException(tableInfo.FullTableName, tableInfo.DbConnectionString, $"Error while creating the TableInfo object: " +
        //            $"{inputParameterValidationResult.ValidationMessage}");
        //    }

        //    var inputPrKeyParameterValidationResult = SqlParameterValidator.ArePrimaryKeysValid(tableInfo, scrambledColumns, constantColumns);
        //    if (!inputPrKeyParameterValidationResult.IsValid)
        //    {
        //        throw new TableInfoException(tableInfo.FullTableName, tableInfo.DbConnectionString, $"Error while creating the TableInfo object: " +
        //            $"{inputPrKeyParameterValidationResult.ValidationMessage}");
        //    }

        //    try
        //    {
        //        tableInfo.SqlToGetConstantColumnTypes = ColumnTypeManager.GenerateColumnTypeSqlQuery(tableInfo, constantColumns);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new TableInfoException(tableInfo.FullTableName, tableInfo.DbConnectionString,
        //            $"Error while generating the sql script for getting constant column types: {ex.Message}", ex);
        //    }

        //    try
        //    {
        //        tableInfo.SqlToGetScrambledColumnTypes = ColumnTypeManager.GenerateColumnTypeSqlQuery(tableInfo, scrambledColumns);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new TableInfoException(tableInfo.FullTableName, tableInfo.DbConnectionString,
        //            $"Error while generating the sql script for getting scrambled column types: {ex.Message}", ex);
        //    }

        //    tableInfo.ConstantColumnsAndValues = constantColumnsAndValues;
        //    tableInfo.ScrambledColumns = scrambledColumns;

        //    return tableInfo;
        //}

        //private string ParseDataSource(string connectionString)
        //{
        //    var connectionStringBuilder = new SqlConnectionStringBuilder(DatabaseConfig.ConnectionString);

        //    return connectionStringBuilder.InitialCatalog;
        //}

        //private (string schemaName, string tableName) ParseSchemaAndTableName(string schemaAndTableName)
        //{
        //    var tableAndSchemaName = schemaAndTableName.Split('.');

        //    return (schemaName: tableAndSchemaName[0], tableName: tableAndSchemaName[1]);
        //}

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

        //private class TableInfo : ITableInfo
        //{
        //    public string DbConnectionString { get; set; }
        //    public string DbName { get; set; }
        //    public string SchemaName { get; set; }
        //    public string TableName { get; set; }
        //    public List<string> ScrambledColumns { get; set; }
        //    public Dictionary<string, string> ConstantColumnsAndValues { get; set; }
        //    public string SqlToGetScrambledColumnTypes { get; set; }
        //    public string SqlToGetConstantColumnTypes { get; set; }
        //    public string WhereClause { get; set; }
        //    public string FullTableName => $"{DbName}.{SchemaName}.{TableName}";
        //}
    }
}
