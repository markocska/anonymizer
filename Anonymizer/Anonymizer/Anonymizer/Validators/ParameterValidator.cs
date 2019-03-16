using Anonymizer.ConstantStores;
using Anonymizer.DbObjects;
using Anonymizer.DbTypes;
using Anonymizer.Properties;
using Anonymizer.Utilities;
using Anonymizer.Validators.ValidationResults;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Anonymizer.Validators
{
    public static class ParameterValidator
    {
        private static void CheckParams(ITableInfo tableInfo, List<string> constantColumns, List<string> scrambledColumns, string paramCheckerScript)
        {
            var constantColumnNamesTable = new ColumnListDataTable();
            foreach (var columnName in constantColumns)
            {
                constantColumnNamesTable.Rows.Add(columnName);
            }


            var scrambledColumnNamesTable = new ColumnListDataTable();
            foreach (var columnName in scrambledColumns)
            {
                scrambledColumnNamesTable.Rows.Add(columnName);
            }

            var checkingQueryParams = new List<SqlParameter>
                 {
                    new SqlParameter() {ParameterName = SqlParameterStore.constColumnsTableParam,
                        SqlDbType = SqlDbType.Structured, Value = constantColumnNamesTable, TypeName = ColumnListDataTable.dbTypeName},
                    new SqlParameter() {ParameterName = SqlParameterStore.scrambledColumnsTableParam,
                        SqlDbType = SqlDbType.Structured, Value = scrambledColumnNamesTable, TypeName = ColumnListDataTable.dbTypeName},
                    new SqlParameter(SqlParameterStore.dbNameParam, tableInfo.DbName),
                    new SqlParameter(SqlParameterStore.schemaNameParam, tableInfo.SchemaName),
                    new SqlParameter(SqlParameterStore.tableNameParam, tableInfo.TableName)
                 };

            SqlHelper.ExecuteNonQuery(tableInfo.DbConnectionString, checkingQueryParams, paramCheckerScript);

        }

        public static ParameterValidationResult AreInputParamsValid(ITableInfo tableInfo, List<string> constantColumns, List<string> scrambledColumns)
        {
            try
            {
                CheckParams(tableInfo, constantColumns, scrambledColumns, Resources.Check_Input_Params);
                return new ParameterValidationResult(true, tableInfo.FullTableName, "");
            }
            catch (Exception ex)
            {
                return new ParameterValidationResult(false, tableInfo.FullTableName, ex.Message);
            }
        }

        public static ParameterValidationResult ArePrimaryKeysValid(ITableInfo tableInfo, List<string> constantColumns, List<string> scrambledColumns)
        {
            try
            {
                CheckParams(tableInfo, constantColumns, scrambledColumns, Resources.Check_Primary_keys);
                return new ParameterValidationResult(true, tableInfo.FullTableName, "");
            }
            catch (Exception ex)
            {
                return new ParameterValidationResult(false, tableInfo.FullTableName, ex.Message);
            }
        }


    }
}
