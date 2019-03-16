using Anonymizer.ConfigCollections;
using Anonymizer.ConfigElements;
using Anonymizer.DbServices;
using Anonymizer.Validators;
using Serilog;
using System;
using System.Collections.Generic;

namespace Anonymizer.Factories
{
    public static class DatabaseConfigFactory
    {
        private static readonly ILogger logger;

        static DatabaseConfigFactory()
        {
            logger = Serilog.Log.ForContext(typeof(DatabaseConfigFactory));
        }

        public static List<DatabaseConfigElement> CreateValidatedDatabaseConfigCollection(DatabaseConfigCollection databases)
        {
            var validatedDatabaseConfigs = new List<DatabaseConfigElement>();
            for (int i = 0; i < databases.Count; i++)
            {
                var dbConfig = databases[i];

                var dbConfigValidationResult = ConfigurationValidator.IsDbConfigValid(dbConfig);

                if (!dbConfigValidationResult.IsValid)
                {
                    logger.Error($"The anonymization of the database with the following connection string was skipped: {dbConfig.ConnectionString}. " +
                    $"Reason: {dbConfigValidationResult.ValidationMessage}");
                }
                else
                {
                    try
                    {
                        DbTypeInitializer.CreateDbTypes(dbConfig.ConnectionString);
                        validatedDatabaseConfigs.Add(databases[i]);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, $"The anonymization of the database with the following connection string was skipped: {dbConfig.ConnectionString}. " +
                        $"Reason: {ex.Message}");
                    }
                }
            }

            return validatedDatabaseConfigs;
        }

    }
}
