using ApplicationCore.Config;
using ApplicationCore.Logging;
using ApplicationCore.Validators.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ApplicationCore.Validators.ParameterValidators
{
    public class SqlParameterValidator : ParameterValidator
    {
        public SqlParameterValidator()
        {
            _logger = Serilog.Log.ForContext<SqlParameterValidator>();
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
                schemaTable = GetTableSchema(connectionString, tableConfig.NameWithSchema);
            }
            catch (SqlException ex)
            {
                _logger.Error($"Error while checking the parameters of table: {tableConfig.NameWithSchema}. " +
                       $"Connection string: {connectionString}. " +
                       $"The mapped database {connectionString} or table {tableConfig.NameWithSchema} doesn't exist or it is unreachable. " +
                       $"Error message: {ex.Message}");
                return false;
            }


            var loggingInfo = new LoggingInfo { ConnectionString = connectionString, TableNameWithSchema = tableConfig.NameWithSchema };

            var constantColumns = tableConfig.ConstantColumns?.Select(c => c.Name)
                .Select(c =>
                {
                    if (c.StartsWith('['))
                    {
                        return c.TrimStart('[').TrimEnd(']');
                    }
                    else
                    {
                        return c;
                    }
                }) ?? new List<string>();

            var scrambledColumns = tableConfig.ScrambledColumns?.Select(c => c.Name)
                .Select(c =>
                {
                    if (c.StartsWith('['))
                    {
                        return c.TrimStart('[').TrimEnd(']');
                    }
                    else
                    {
                        return c;
                    }
                }) ?? new List<string>();

            var pairedColumns = tableConfig.PairedColumnsInsideTable?
                .Select(cl =>
                    cl.Select(c =>
                    {
                        if (c.StartsWith('['))
                        {
                            return c.TrimStart('[').TrimEnd(']');
                        }
                        else
                        {
                            return c;
                        }
                    }).ToList()).ToList() ?? new List<List<string>>();

            var allColumns = constantColumns.Concat(scrambledColumns);

            doConstantScrambledDuplicatesExist = DoConstantScrambledDuplicatesExist(loggingInfo, scrambledColumns, constantColumns);
            doAllColumnsExist = DoAllColumnsExist(schemaTable, loggingInfo, allColumns);
            doAllPairedColumnsInsideExist = DoAllPairedColumnsInsideExist(loggingInfo, schemaTable, pairedColumns);
            doAllPairedColumnsOutsideExist = DoAllPairedColumnsOutsideExist(connectionString, loggingInfo, tableConfig);
            isThereAPrimaryKeyConflict = IsThereAPrimaryKeyConflict(schemaTable, loggingInfo, allColumns);
            isThereAUniqueConstraintConflict = IsThereAUniqueConstraintConflict(schemaTable, loggingInfo, allColumns);

            return (!doConstantScrambledDuplicatesExist && doAllColumnsExist && doAllPairedColumnsInsideExist &&
                doAllPairedColumnsOutsideExist && !isThereAPrimaryKeyConflict && !isThereAUniqueConstraintConflict);



        }

        protected override DataTable GetTableSchema(string connectionString, string nameWithSchema)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            using (var adapter = new SqlDataAdapter("select * from " + nameWithSchema + ";", sqlConnection))
            using (var tableMetadata = new DataTable(nameWithSchema))
            {
                return adapter
                       .FillSchema(tableMetadata, SchemaType.Mapped);
            }
        }
    }
}
