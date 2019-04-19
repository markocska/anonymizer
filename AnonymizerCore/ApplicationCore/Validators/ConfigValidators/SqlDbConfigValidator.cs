using ApplicationCore.Config;
using Serilog;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

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
            var tableAndSchemaName = tableConfig.NameWithSchema.Split('.');

            if (tableAndSchemaName.Length != 2)
            {
                _logger.Error($"The following schema and name config parameter is invalid {tableConfig.NameWithSchema}.",
                    tableConfig, dbConfig);
                return false;
            }

            if (tableConfig.ScrambledColumns.Count == 0 && tableConfig.ConstantColumns.Count == 0)
            {
                _logger.Error($"The following table has no columns to anonymize: {tableConfig.NameWithSchema}",
                    tableConfig, dbConfig);
                return false;
            }

            return true;
        }

        public bool IsDbConfigValid(DatabaseConfig dbConfig)
        {
            try
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder(dbConfig.ConnectionString);

                if (connectionStringBuilder.InitialCatalog == string.Empty)
                {
                    _logger.Error($"The following connection string doesn't contain an initial catalog: {dbConfig.ConnectionString}.",
                        dbConfig);
                    return false;
                }

                if (connectionStringBuilder.DataSource == string.Empty)
                {
                    _logger.Error($"The following connection string doesn't contain a data source {dbConfig.ConnectionString}.",
                        dbConfig);
                    return false;
                }

                if (dbConfig.Tables == null)
                {
                    _logger.Error($"The database with the connection string {dbConfig.ConnectionString} has a Tables property of null.",
                        dbConfig);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"The connection string : {dbConfig.ConnectionString} has an invalid format. " +
                    $"Error message: {ex.Message}.", dbConfig.ConnectionString);
                return false;
            }

            return true;
        }

    }
}
