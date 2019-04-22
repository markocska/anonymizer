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

            using (var sqlConnection = new SqlConnection(connectionString))
            using (var adapter = new SqlDataAdapter("select * from " + tableConfig.NameWithSchema + ";", sqlConnection))
            using (var tableMetadata = new DataTable(tableConfig.NameWithSchema))
            {
                try
                {
                    var schemaTable = adapter
                       .FillSchema(tableMetadata, SchemaType.Mapped);

                    var loggingInfo = new LoggingInfo { ConnectionString = connectionString, TableNameWithSchema = tableConfig.NameWithSchema };

                    var constantColumns = tableConfig.ConstantColumns.Select(c => c.Name)
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
                        });

                    var scrambledColumns = tableConfig.ScrambledColumns.Select(c => c.Name)
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
                        });

                    var pairedColumns = tableConfig.PairedColumnsInsideTable
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
                            }).ToList()).ToList();

                    var allColumns = constantColumns.Concat(scrambledColumns);

                    var doConstantScrambledDuplicatesExist = DoConstantScrambledDuplicatesExist(loggingInfo, scrambledColumns, constantColumns);
                    var doAllColumnsExist = DoAllColumnsExist(schemaTable, loggingInfo, allColumns);
                    var doAllPairedColumnsInsideExist = DoAllPairedColumnsInsideExist(loggingInfo, schemaTable, pairedColumns);
                    var isThereAPrimaryKeyConflict = IsThereAPrimaryKeyConflict(schemaTable, loggingInfo, allColumns);
                    var isThereAUniqueConstraintConflict = IsThereAUniqueConstraintConflict(schemaTable, loggingInfo, allColumns);

                    return (!doConstantScrambledDuplicatesExist && doAllColumnsExist && doAllPairedColumnsInsideExist &&
                        !isThereAPrimaryKeyConflict && !isThereAUniqueConstraintConflict);

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
