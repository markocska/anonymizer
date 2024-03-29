﻿using Microsoft.Extensions.Logging;
using Scrambler.Config;
using Serilog;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Scrambler.Validators.ConfigValidators
{
    public class SqlConfigValidator : IConfigValidator
    {
        public bool IsTableConfigValid(DatabaseConfig dbConfig, TableConfig tableConfig)
        {
            if (dbConfig == null) { throw new ArgumentNullException("The db config parameter can not be null"); }
            if (tableConfig == null) { throw new ArgumentNullException("The table config parameter can not be null"); }

            if (!IsTableNameValid(dbConfig.ConnectionString, tableConfig.FullTableName))
            {
                return false;
            }

            if (tableConfig.ScrambledColumns.Count == 0 && tableConfig.ConstantColumns.Count == 0)
            {
                Log.Error($"The following table has no columns to anonymize: {tableConfig.FullTableName}",
                    tableConfig, dbConfig);
                return false;
            }

            if (tableConfig.PairedColumnsOutsideTable != null && tableConfig.PairedColumnsOutsideTable.Count != 0 
                && tableConfig.PairedColumnsOutsideTable.First().ColumnMapping.Count != 0)
            {
                foreach (var pairedColumnConfig in tableConfig.PairedColumnsOutsideTable)
                {
                    if (pairedColumnConfig.ColumnMapping.Any(l => l.Count != 2))
                    {

                        Log.Error($"Error while checking the paired columns outside of table {tableConfig.FullTableName}. " +
                                $"Connection string: {dbConfig.ConnectionString}. " +
                                $"The column mapping arrays must consist of 2 columns.");
                        return false;
                    }

                    foreach (var connectedTableConfig in pairedColumnConfig.SourceDestMapping)
                    {
                        if (connectedTableConfig.ForeignKeyMapping.Any(l => l.Count != 2))
                        {
                            Log.Error($"Error while checking the paired columns outside of table {tableConfig.FullTableName}. " +
                               $"Connection string: {dbConfig.ConnectionString}. " +
                               $"The foreign key mapping arrays must consist of 2 columns for mapped table {connectedTableConfig.DestinationFullTableName}. " +
                               $"Connection string: {connectedTableConfig.DestinationConnectionString}.");
                            return false;
                        }

                        if (!IsTableNameValid(connectedTableConfig.DestinationConnectionString, connectedTableConfig.DestinationFullTableName))
                        {
                            Log.Error($"Error while checking the paired columns outside of table {tableConfig.FullTableName}. " +
                                $"Connection string: {dbConfig.ConnectionString}.");
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private bool IsTableNameValid(string connectionString, string tableNameWithSchema)
        {
            var tableAndSchemaName = tableNameWithSchema.Split('.');

            if (tableAndSchemaName.Length != 3 && tableAndSchemaName.Length != 4)
            {
                Log.Error($"The following full table config parameter is invalid {tableNameWithSchema}." +
                    $" Connection string: {connectionString}");
                return false;
            }

            return true;
        }

        public bool IsDbConfigValid(DatabaseConfig dbConfig)
        {
            if (dbConfig == null)
            {
                throw new ArgumentNullException("The dbConfigParameter can not be null.");
            }

            if (!IsConnectionStringValid(dbConfig.ConnectionString))
            {
                return false;
            }

            if (dbConfig.Tables == null)
            {
                Log.Error($"The database with the connection string {dbConfig.ConnectionString} has a Tables property of null.",
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
                    Log.Error($"The following connection string doesn't contain an initial catalog: {connectionString}.",
                        connectionString);
                    return false;
                }

                if (connectionStringBuilder.DataSource == string.Empty)
                {
                    Log.Error($"The following connection string doesn't contain a data source {connectionString}.",
                        connectionString);
                    return false;
                }

            }
            catch (Exception ex)
            {
                Log.Error($"The connection string : {connectionString} has an invalid format. " +
                    $"Error message: {ex.Message}.", connectionString);
                return false;
            }

            return true;
        }

    }
}
