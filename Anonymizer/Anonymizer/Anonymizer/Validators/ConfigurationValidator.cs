using Anonymizer.ConfigElements;
using Anonymizer.Validators.ValidationResults;
using System;
using System.Data.SqlClient;

namespace Anonymizer.Validators
{
    public static class ConfigurationValidator
    {
        public static TableValidationResult IsTableConfigValid(DatabaseConfigElement dbConfig, TableConfigElement tableConfig)
        {
            var dbConfigValidationResult = IsDbConfigValid(dbConfig);
            if (!dbConfigValidationResult.IsValid)
            {
                return new TableValidationResult(false, dbConfigValidationResult.ValidationMessage, tableConfig.NameWithSchema, dbConfig.ConnectionString);
            }

            var tableAndSchemaName = tableConfig.NameWithSchema.Split('.');

            if (tableAndSchemaName.Length != 2)
            {
                return new TableValidationResult(false, $"The following schema and name config parameter is invalid {tableConfig.NameWithSchema}.", tableConfig.NameWithSchema, dbConfig.ConnectionString);
            }

            if (tableConfig.ScrambledColumns.Count == 0 && tableConfig.ConstantColumns.Count == 0)
            {
                return new TableValidationResult(false, $"The following table has no columns to anonymize: {tableConfig.NameWithSchema}", tableConfig.NameWithSchema, dbConfig.ConnectionString);
            }

            return new TableValidationResult(true, "", tableConfig.NameWithSchema, dbConfig.ConnectionString);
        }


        public static DatabaseValidationResult IsDbConfigValid(DatabaseConfigElement dbConfig)
        {
            try
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder(dbConfig.ConnectionString);


                if (connectionStringBuilder.InitialCatalog == string.Empty)
                {
                    return new DatabaseValidationResult(false, $"The following connection string doesn't contain an initial catalog: {dbConfig.ConnectionString}.", dbConfig.ConnectionString);
                }

                if (connectionStringBuilder.DataSource == string.Empty)
                {
                    return new DatabaseValidationResult(false, $"The following connection string doesn't contain a data source {dbConfig.ConnectionString}.", dbConfig.ConnectionString);
                }

                if (dbConfig.Tables == null)
                {
                    return new DatabaseValidationResult(false, $"The database with the connection string {dbConfig.ConnectionString} has a Tables property of null.", dbConfig.ConnectionString);
                }

            }
            catch (Exception ex)
            {
                return new DatabaseValidationResult(false, $"The connection string : {dbConfig.ConnectionString} has an invalid format. Error message: {ex.Message}.", dbConfig.ConnectionString);
            }

            return new DatabaseValidationResult(true, "", dbConfig.ConnectionString);
        }
    }
}
