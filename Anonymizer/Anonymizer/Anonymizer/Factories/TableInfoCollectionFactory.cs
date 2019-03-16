using Anonymizer.Builders;
using Anonymizer.Config;
using Anonymizer.DbObjects;
using Anonymizer.Exceptions;
using Serilog;
using System;
using System.Collections.Generic;

namespace Anonymizer.Factories
{
    public static class TableInfoCollectionFactory
    {
        private static readonly ILogger logger;

        static TableInfoCollectionFactory()
        {
            logger = Serilog.Log.ForContext(typeof(TableInfoCollectionFactory));
        }

        public static List<ITableInfo> CreateTableListFromConfig(DatabaseConfigurationSection config)
        {
            var tablesToAnonymize = new List<ITableInfo>();
            if (config == null)
            {
                throw new ArgumentNullException("The DatabaseConfigurationSection input parameter is null.");
            }

            var dbs = config.Databases;
            if (dbs == null)
            {
                return tablesToAnonymize;
            }

            var validatedDatabaseConfigs = DatabaseConfigFactory.CreateValidatedDatabaseConfigCollection(config.Databases);
            foreach (var dbConfig in validatedDatabaseConfigs)
            {
                var validatedTableConfigs = TableConfigFactory.CreateValidatedTableConfigCollection(dbConfig);
                foreach (var tableConfig in validatedTableConfigs)
                {
                    try
                    {
                        var tableInfo = new TableInfoBuilder(dbConfig, tableConfig).Build();
                        tablesToAnonymize.Add(tableInfo);
                    }

                    catch (TableInfoException ex)
                    {
                        logger.Error(ex, $"The table with name {ex.TableName} with connection string {ex.ConnectionString} couldn't be anonymized. Error reason: " +
                            $"{ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, ex.StackTrace);
                    }
                }
            }

            return tablesToAnonymize;
        }
    }
}
