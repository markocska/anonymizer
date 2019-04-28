using ApplicationCore.Config;
using Serilog;
using System;
using System.Data.SqlClient;
using System.Linq;

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
            if (dbConfig == null) { throw new ArgumentNullException("The db config parameter can not be null"); }
            if (tableConfig == null) { throw new ArgumentNullException("The table config parameter can not be null"); }

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

            if (tableConfig.PairedColumnsOutsideTable != null)
            {
                foreach (var pairedColumnConfig in tableConfig.PairedColumnsOutsideTable)
                {
                    if (pairedColumnConfig.ColumnMapping.Any(l => l.Count != 2))
                    {

                        _logger.Error($"Error while checking the paired columns outside of table {tableConfig.NameWithSchema}. " +
                                $"Connection string: {dbConfig.ConnectionString}. " +
                                $"The column mapping arrays must consist of 2 columns.");
                        return false;
                    }

                    foreach (var connectedTableConfig in pairedColumnConfig.SourceDestMapping)
                    {
                        if (connectedTableConfig.ForeignKeyMapping.Any(l => l.Count != 2))
                        {
                            _logger.Error($"Error while checking the paired columns outside of table {tableConfig.NameWithSchema}. " +
                               $"Connection string: {dbConfig.ConnectionString}. " +
                               $"The foreign key mapping arrays must consist of 2 columns for mapped table {connectedTableConfig.DestinationTableNameWithSchema}. " +
                               $"Connection string: {connectedTableConfig.DestinationConnectionString}.");
                            return false;
                        }

                        if (!IsConnectionStringValid(connectedTableConfig.DestinationConnectionString) ||
                            !IsTableNameValid(connectedTableConfig.DestinationConnectionString, connectedTableConfig.DestinationTableNameWithSchema))
                        {
                            _logger.Error($"Error while checking the paired columns outside of table {tableConfig.NameWithSchema}. " +
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
            if (dbConfig == null)
            {
                throw new ArgumentNullException("The dbConfigParameter can not be null");
            }

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
