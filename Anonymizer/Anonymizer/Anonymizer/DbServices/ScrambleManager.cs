using Anonymizer.ConstantStores;
using Anonymizer.DbObjects;
using Anonymizer.DbTypes;
using Anonymizer.Properties;
using Anonymizer.Utilities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Anonymizer.DbServices
{
    public static class ScrambleManager
    {
        private static readonly ILogger logger;

        public static string SuccessLog { get; private set; }

        static ScrambleManager()
        {
            logger = Serilog.Log.ForContext(typeof(ScrambleManager));
        }

        public static bool Scramble(ITableInfo tableInfo)
        {
            var constantColumnsAndValuesTable = new ColumnAndValueListDataTable();
            foreach (var columnValuePair in tableInfo.ConstantColumnsAndValues)
            {
                constantColumnsAndValuesTable.Rows.Add(columnValuePair.Key, columnValuePair.Value);
            }

            var scrambledColumnsTable = new ColumnListDataTable();
            foreach (var column in tableInfo.ScrambledColumns)
            {
                scrambledColumnsTable.Rows.Add(column);
            }

            var getScramblingScriptParams = new List<SqlParameter>
            {
            new SqlParameter() {ParameterName = SqlParameterStore.constColumnsAndValuesTableParam, SqlDbType = SqlDbType.Structured,
                Value = constantColumnsAndValuesTable, TypeName = ColumnAndValueListDataTable.dbTypeName},
            new SqlParameter() {ParameterName = SqlParameterStore.scrambledColumnsTableParam, SqlDbType = SqlDbType.Structured,
                Value = scrambledColumnsTable, TypeName = ColumnListDataTable.dbTypeName},
            new SqlParameter(SqlParameterStore.sqlToGetConstantTypesParam, tableInfo.SqlToGetConstantColumnTypes),
            new SqlParameter(SqlParameterStore.sqlToGetScrambledTypesParam, tableInfo.SqlToGetScrambledColumnTypes),
            new SqlParameter(SqlParameterStore.dbNameParam, tableInfo.DbName),
            new SqlParameter(SqlParameterStore.schemaNameParam, tableInfo.SchemaName),
            new SqlParameter(SqlParameterStore.tableNameParam, tableInfo.TableName),
            new SqlParameter(SqlParameterStore.whereParam, tableInfo.WhereClause)
            };

            try
            {
                IndexManager.TurnIndexesOnOff(tableInfo, false);
                SqlHelper.ExecuteNonQuery(tableInfo.DbConnectionString, getScramblingScriptParams, Resources.Scramble);
                IndexManager.TurnIndexesOnOff(tableInfo, true);

                LogSuccess(tableInfo);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"An error happened while scrambling the table {tableInfo.FullTableName}: {ex.Message}");
                return false;
            }
        }

        private static void LogSuccess(ITableInfo tableInfo)
        {
            int lastComma = 0;

            string scrambledColumnsStr = "";
            tableInfo.ScrambledColumns.ForEach(c => { scrambledColumnsStr += c + ", "; });
            lastComma = scrambledColumnsStr.LastIndexOf(',');
            scrambledColumnsStr = lastComma == -1 ? "None" : scrambledColumnsStr.Substring(0, lastComma);

            string constantColumnsStr = "";
            foreach (var columnAndValue in tableInfo.ConstantColumnsAndValues)
            {
                constantColumnsStr += $"{columnAndValue.Key} = {columnAndValue.Value}, ";
            }
            lastComma = constantColumnsStr.LastIndexOf(',');
            constantColumnsStr = lastComma == -1 ? "None" : constantColumnsStr.Substring(0, lastComma);

            logger.Verbose($"The following table has been anonymized successfully: {tableInfo.FullTableName}. " +
                $"Scrambled columns: {scrambledColumnsStr}. " +
                $"Constant columns set: {constantColumnsStr} ");
        }
    }
}
