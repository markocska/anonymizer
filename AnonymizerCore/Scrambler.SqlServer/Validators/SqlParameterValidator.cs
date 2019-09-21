using Microsoft.Extensions.Logging;
using Scrambler.Config;
using Scrambler.Logging;
using Scrambler.Utilities;
using Scrambler.Validators.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Scrambler.Validators.ParameterValidators
{
    public class SqlParameterValidator : ParameterValidator
    {
        private readonly ILogger<SqlParameterValidator> _logger;

        public SqlParameterValidator(ILogger<SqlParameterValidator> logger) : base(logger)
        {
            _logger = logger;
        }

        public override bool AreTheParamsValid(string connectionString, TableConfig tableConfig)
        {
            bool doConstantScrambledDuplicatesExist;
            bool doAllColumnsExist;
            bool doAllPairedColumnsInsideExist;
            bool doAllPairedColumnsOutsideExist;
            bool isThereAPrimaryKeyConflict;
            bool isThereAUniqueConstraintConflict;

            DataTable schemaTable;
            try
            {
                schemaTable = GetTableSchema(connectionString, tableConfig.FullTableName);
            }
            catch (SqlException ex)
            {
                _logger.LogError($"Error while checking the parameters of table: {tableConfig.FullTableName}. " +
                       $"Connection string: {connectionString}. " +
                       $"The mapped database {connectionString} or table {tableConfig.FullTableName} doesn't exist or it is unreachable. " +
                       $"Error message: {ex.Message}");
                return false;
            }


            var loggingInfo = new LoggingInfo { ConnectionString = connectionString, TableNameWithSchema = tableConfig.FullTableName };

            var constantColumns = tableConfig.ConstantColumns?.Select(c => c.Name)
                .Select(c => ParameterNameHelper.RemoveParenthesises(c)) ?? new List<string>();

            var scrambledColumns = tableConfig.ScrambledColumns?.Select(c => c.Name)
                .Select(c => ParameterNameHelper.RemoveParenthesises(c)) ?? new List<string>();

            var pairedColumns = tableConfig.PairedColumnsInsideTable?
                .Select(cl =>
                    cl.Select(c => ParameterNameHelper.RemoveParenthesises(c)).ToList()).ToList() ?? new List<List<string>>();

            var allColumns = constantColumns.Concat(scrambledColumns);

            doConstantScrambledDuplicatesExist = DoConstantScrambledDuplicatesExist(loggingInfo, scrambledColumns, constantColumns);
            doAllColumnsExist = DoAllColumnsExist(schemaTable, loggingInfo, allColumns);
            doAllPairedColumnsInsideExist = DoAllPairedColumnsInsideExist(loggingInfo, schemaTable, pairedColumns);
            doAllPairedColumnsOutsideExist = DoAllPairedColumnsOutsideExist(connectionString, tableConfig);
            isThereAPrimaryKeyConflict = IsThereAPrimaryKeyConflict(schemaTable, loggingInfo, allColumns);
            isThereAUniqueConstraintConflict = IsThereAUniqueConstraintConflict(schemaTable, loggingInfo, allColumns);

            return (!doConstantScrambledDuplicatesExist && doAllColumnsExist && doAllPairedColumnsInsideExist &&
                doAllPairedColumnsOutsideExist && !isThereAPrimaryKeyConflict && !isThereAUniqueConstraintConflict);

        }

        protected override IEnumerable<string> GetNormalizedColumnListCopy(IEnumerable<string> columns)
        {
            return ParameterNameHelper.RemoveParenthesisesFromStringList(columns.ToList());
        }

        protected override DataTable GetTableSchema(string connectionString, string fullTableName)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            using (var adapter = new SqlDataAdapter("select * from " + fullTableName + ";", sqlConnection))
            using (var tableMetadata = new DataTable(fullTableName))
            {
                return adapter
                       .FillSchema(tableMetadata, SchemaType.Mapped);
            }
        }
    }
}
