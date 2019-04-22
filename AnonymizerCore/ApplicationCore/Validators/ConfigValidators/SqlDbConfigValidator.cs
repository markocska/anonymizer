﻿using ApplicationCore.Config;
using Serilog;
using System;
using System.Data.SqlClient;

namespace ApplicationCore.Validators.ConfigValidators
{
    public class SqlConfigValidator : IConfigValidator
    {
        private readonly ILogger _logger;

        public SqlConfigValidator()
        {
            _logger = Serilog.Log.ForContext(typeof(SqlConfigValidator));
        }

        public bool IsTableConfigValid(DatabaseConfig dbConfig, TableConfig tableConfig)
        {
            if (!IsTableNameValid(dbConfig.ConnectionString, tableConfig.NameWithSchema))
            {
                return false;
            }

            if (tableConfig.ScrambledColumns.Count == 0 && tableConfig.ConstantColumns.Count == 0)
            {
                _logger.Error($"The following table has no columns to anonymize: {tableConfig.NameWithSchema}",
                    tableConfig, dbConfig);
                return false;
            }

            foreach (var pairedColumnConfig in tableConfig.PairedColumnsOutsideTable)
            {
                foreach (var connectedTableConfig in pairedColumnConfig.SourceDestMapping)
                {
                    if (!IsConnectionStringValid(connectedTableConfig.ConnectionString) ||
                        !IsTableNameValid(connectedTableConfig.ConnectionString, connectedTableConfig.TableNameWithSchema))
                    {
                        _logger.Error($"Error while checking the paired columns outside of table {tableConfig.NameWithSchema}. " +
                            $"Connection string: {dbConfig.ConnectionString}.");
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsTableNameValid(string connectionString, string tableNameWithSchema)
        {
            var tableAndSchemaName = tableNameWithSchema.Split('.');

            if (tableAndSchemaName.Length != 2)
            {
                _logger.Error($"The following schema and name config parameter is invalid {tableNameWithSchema}." +
                    $" Connection string: {connectionString}");
                return false;
            }

            return true;
        }

        public bool IsDbConfigValid(DatabaseConfig dbConfig)
        {
            if (!IsConnectionStringValid(dbConfig.ConnectionString))
            {
                return false;
            }

            if (dbConfig.Tables == null)
            {
                _logger.Error($"The database with the connection string {dbConfig.ConnectionString} has a Tables property of null.",
                    dbConfig);
                return false;
            }

            return true;
        }

        private bool IsConnectionStringValid(string connectionString)
        {
            try
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

                if (connectionStringBuilder.InitialCatalog == string.Empty)
                {
                    _logger.Error($"The following connection string doesn't contain an initial catalog: {connectionString}.",
                        connectionString);
                    return false;
                }

                if (connectionStringBuilder.DataSource == string.Empty)
                {
                    _logger.Error($"The following connection string doesn't contain a data source {connectionString}.",
                        connectionString);
                    return false;
                }

            }
            catch (Exception ex)
            {
                _logger.Error($"The connection string : {connectionString} has an invalid format. " +
                    $"Error message: {ex.Message}.", connectionString);
                return false;
            }

            return true;
        }

    }
}