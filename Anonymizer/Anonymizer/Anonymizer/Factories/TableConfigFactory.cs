using Anonymizer.ConfigElements;
using Anonymizer.Validators;
using Serilog;
using System.Collections.Generic;

namespace Anonymizer.Factories
{
    public static class TableConfigFactory
    {
        private static readonly ILogger logger;

        static TableConfigFactory()
        {
            logger = Serilog.Log.ForContext(typeof(TableConfigFactory));
        }

        public static List<TableConfigElement> CreateValidatedTableConfigCollection(DatabaseConfigElement dbConfig)
        {
            var validatedTableConfigs = new List<TableConfigElement>();

            var dbConfigValidationResult = ConfigurationValidator.IsDbConfigValid(dbConfig);
            if (!dbConfigValidationResult.IsValid)
            {
                logger.Error($"The anonymization of the database with the following connection string was skipped: {dbConfig.ConnectionString}. " +
                    $"Reason: {dbConfigValidationResult.ValidationMessage}");
                return validatedTableConfigs;
            }


            for (int i = 0; i < dbConfig.Tables.Count; i++)
            {
                var tableConfig = dbConfig.Tables[i];

                var tableConfigValidationResult = ConfigurationValidator.IsTableConfigValid(dbConfig, tableConfig);
                if (!tableConfigValidationResult.IsValid)
                {
                    logger.Error($"The anonymization of the table {tableConfigValidationResult.TableNameWithSchema} " +
                        $"with the following connection string was skipped: {tableConfigValidationResult.ConnectionString}. " +
                        $"Reason: {tableConfigValidationResult.ValidationMessage}");
                }
                else
                {
                    validatedTableConfigs.Add(tableConfig);
                }

            }

            return validatedTableConfigs;
        }

    }
}
