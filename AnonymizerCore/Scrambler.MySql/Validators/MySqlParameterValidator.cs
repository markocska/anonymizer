using MySql.Data.MySqlClient;
using Scrambler.Config;
using Scrambler.Logging;
using Scrambler.Validators.Abstract;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Scrambler.MySql.Validators
{
    public class MySqlParameterValidator : ParameterValidator
    {
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
            catch (MySqlException ex)
            {
                Log.Error($"Error while checking the parameters of table: {tableConfig.FullTableName}. " +
                       $"Connection string: {connectionString}. " +
                       $"The mapped database {connectionString} or table {tableConfig.FullTableName} doesn't exist or it is unreachable. " +
                       $"Error message: {ex.Message}");
                return false;
            }


            var loggingInfo = new LoggingInfo { ConnectionString = connectionString, TableNameWithSchema = tableConfig.FullTableName };

            var constantColumns = tableConfig.ConstantColumns?.Select(c => c.Name)
            .Select(c => ParameterNameHelper.RemoveQuotationMarks(c)) ?? new List<string>();

            var scrambledColumns = tableConfig.ScrambledColumns?.Select(c => c.Name)
                .Select(c => ParameterNameHelper.RemoveQuotationMarks(c)) ?? new List<string>();

            var pairedColumns = tableConfig.PairedColumnsInsideTable?
                .Select(cl =>
                    cl.Select(c => ParameterNameHelper.RemoveQuotationMarks(c)).ToList()).ToList() ?? new List<List<string>>();

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
            return columns;
        }

        protected override DataTable GetTableSchema(string connectionString, string fullTableName)
        {
            using (var sqlConnection = new MySqlConnection(connectionString))
            using (var adapter = new MySqlDataAdapter("select * from " + fullTableName + ";", sqlConnection))
            using (var tableMetadata = new DataTable(fullTableName))
            {
                return adapter
                       .FillSchema(tableMetadata, SchemaType.Mapped);
            }
        }
    }
}
