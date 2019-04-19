using ApplicationCore.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ApplicationCore.Validators.ParameterValidators
{
    public class SqlParameterValidator
    {
        ILogger _logger;
        public SqlParameterValidator()
        {
            _logger = Serilog.Log.ForContext<SqlParameterValidator>();
        }

        public bool CheckParams(string connectionString, string tableNameWithSchema,
            List<string> constantColumns, List<string> scrambledColumns)
        {
            bool isThereAnyError = false;

            using (var sqlConnection = new SqlConnection(connectionString))
            using (var adapter = new SqlDataAdapter("select * from " + tableNameWithSchema + ";", sqlConnection))
            using (var tableMetadata = new DataTable(tableNameWithSchema))
            {
                var schemaTable = adapter
                   .FillSchema(tableMetadata, SchemaType.Mapped);

                var loggingInfo = new LoggingInfo { ConnectionString = connectionString, TableNameWithSchema = tableNameWithSchema };
                var allColumns = constantColumns.Concat(scrambledColumns);

                isThereAnyError = DoAllColumnsExist(schemaTable, loggingInfo, allColumns) ? false : true;
                isThereAnyError = IsThereAPrimaryKeyConflict(schemaTable, loggingInfo, allColumns);
                isThereAnyError = IsThereAUniqueConstraintConflict(schemaTable, loggingInfo, allColumns);
            }

            return isThereAnyError;
        }

        private bool DoAllColumnsExist(DataTable schemaTable, LoggingInfo logInfo, IEnumerable<string> columns)
        {
            var allColumns = schemaTable
                .Columns;

            bool doAllColumnsExist = true;
            foreach (var column in columns)
            {
                if (allColumns[column] == null)
                {
                    _logger.Error($"The column {column} doesn't exist in the table.", logInfo);
                    doAllColumnsExist = false;
                }
            }

            return doAllColumnsExist;
        }

        private bool IsThereAUniqueConstraintConflict(DataTable schemaTable, LoggingInfo logInfo, IEnumerable<string> columns)
        {
            var allColumns = schemaTable
                   .Columns;

            bool isThereAUniqueConstraintConflict = false;
            foreach (var column in columns)
            {
                if (allColumns[column] == null)
                {
                    continue;
                }

                if (allColumns[column].Unique)
                {
                    _logger.Error($"The column {column} doesn't exist in the table.", logInfo);
                    isThereAUniqueConstraintConflict = true;
                }
            }

            return isThereAUniqueConstraintConflict;
        }

        private bool IsThereAPrimaryKeyConflict(DataTable schemaTable, LoggingInfo logInfo, IEnumerable<string> columns)
        {
            var primaryKeys = schemaTable.PrimaryKey.Select(c => c.ColumnName);

            bool isThereAPrimaryKeyConflict = false;
            foreach (var primaryKey in primaryKeys)
            {
                if (columns.Contains(primaryKey))
                {
                    _logger.Error($"The following column is part of a primary key: {primaryKey} .",
                        logInfo);
                    isThereAPrimaryKeyConflict = true;
                }
            }

            return isThereAPrimaryKeyConflict;

        }
    }
}
