using Microsoft.Extensions.Logging;
using Scrambler.Config;
using Scrambler.Utilities;
using Scrambler.Utilities.QueryHelpers;
using Scrambler.Validators.Abstract;
using Scrambler.Validators.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Scrambler.SqlServer.Validators
{
    public class SqlWhereConditionValidator : IWhereConditionValidator
    {
        private readonly ILogger<SqlWhereConditionValidator> _logger;
        private readonly IQueryHelper _sqlHelper;

        public SqlWhereConditionValidator(ILogger<SqlWhereConditionValidator> logger, IQueryHelper sqlHelper)
        {
            _logger = logger;
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
            catch(SqlException ex)
            {
                _logger.LogError($"Error while checking the where condition of table {tableConfig.FullTableName}. Connection string: {connectionString}." +
                    $"Message: {ex.Message}.");
                return false;
            }
        }
    }
}
