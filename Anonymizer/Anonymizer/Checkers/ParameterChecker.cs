using Anonymizer.Config;
using Anonymizer.ConstantStores;
using Anonymizer.DbObjects;
using Anonymizer.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anonymizer.Checkers
{
    public static class ParameterChecker
    {
        public static bool CheckInputParams(TableInfo tableInfo, List<string> constantColumns, List<string> scrambledColumns)
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

            try
            {
                SqlHelper.ExecuteNonQuery(tableInfo.DbConnectionString, checkingQueryParams, Resources.Check_Input_Params);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public static bool CheckPrimaryKeys(TableInfo tableInfo, List<string> constantColumns, List<string> scrambledColumns)
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

            try
            {
                SqlHelper.ExecuteNonQuery(tableInfo.DbConnectionString, checkingQueryParams, Resources.Check_Primary_keys);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

    }
}
