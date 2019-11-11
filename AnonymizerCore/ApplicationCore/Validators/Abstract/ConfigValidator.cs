using Scrambler.Config;
using Serilog;
using System;
using System.Linq;

namespace Scrambler.Validators.Abstract
{
    public abstract class ConfigValidator : IConfigValidator
    {
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

        public virtual bool IsTableConfigValid(DatabaseConfig dbConfig, TableConfig tableConfig)
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

            if (tableConfig.PairedColumnsOutsideTable != null)
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

        protected abstract bool IsTableNameValid(string connectionString, string tableNameWithSchema);

        protected abstract bool IsConnectionStringValid(string connectionString);
    }
}
