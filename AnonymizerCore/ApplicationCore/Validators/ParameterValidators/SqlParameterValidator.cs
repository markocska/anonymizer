using ApplicationCore.Config;
using ApplicationCore.Logging;
using ApplicationCore.Validators.Abstract;
using Serilog;
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

            using (var sqlConnection = new SqlConnection(connectionString))
            using (var adapter = new SqlDataAdapter("select * from " + tableConfig.NameWithSchema + ";", sqlConnection))
            using (var tableMetadata = new DataTable(tableConfig.NameWithSchema))
            {
                try
                {
                    var schemaTable = adapter
                       .FillSchema(tableMetadata, SchemaType.Mapped);

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
                catch (Exception ex)
                {
                    _logger.Error($"There was an error while connecting to the server. {ex.Message}.", ex);
                    throw;
                }
            }
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
