using MySql.Data.MySqlClient;
using Scrambler.Config;
using Scrambler.Utilities;
using Scrambler.Validators.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.MySql.Validators
{
    public class MySqlWhereConditionValidator : IWhereConditionValidator
    {

        private readonly IQueryHelper _sqlHelper;

        public MySqlWhereConditionValidator(IQueryHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        public bool IsWhereConditionValid(string connectionString, TableConfig tableConfig)
        {
            if (string.IsNullOrEmpty(tableConfig.Where))
            {
                return true;
            }

            try
            {
                _sqlHelper.ExecuteQueryWithoutParams(connectionString, $"select * from {tableConfig.FullTableName} where {tableConfig.Where};");
                return true;
            }
            catch (MySqlException ex)
            {
                Log.Error($"Error while checking the where condition of table {tableConfig.FullTableName}. Connection string: {connectionString}." +
                    $"Message: {ex.Message}.");
                return false;
            }
        }
    }
}
