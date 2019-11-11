using MySql.Data.MySqlClient;
using Scrambler.Config;
using Scrambler.Validators;
using Scrambler.Validators.Abstract;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.MySql.Validators
{
    public class MySqlConfigValidator : ConfigValidator
    {

        protected override bool IsTableNameValid(string connectionString, string tableNameWithSchema)
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

        protected override bool IsConnectionStringValid(string connectionString)
        {
            try
            {
                var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);

                if (connectionStringBuilder.Database == string.Empty)
                {
                    Log.Error($"The following connection string doesn't contain an initial database: {connectionString}.",
                        connectionString);
                    return false;
                }

                if (connectionStringBuilder.Server == string.Empty)
                {
                    Log.Error($"The following connection string doesn't contain a server name {connectionString}.",
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
