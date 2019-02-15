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

namespace Anonymizer
{
    public static class ScrambleManager
    {
        public static bool Scramble(TableInfo tableInfo, Dictionary<string,string> constantColumnsAndValuesDict, List<string> scrambledColumnsList,
                                    string sqlToGetConstantTypes, string sqlToGetScrambledTypes, string whereClause)
        {
            var constantColumnsAndValuesTable = new ColumnAndValueListDataTable();
            foreach(var columnValuePair in constantColumnsAndValuesDict)
            {
                constantColumnsAndValuesTable.Rows.Add(columnValuePair.Key, columnValuePair.Value);
            }

            var scrambledColumnsTable = new ColumnListDataTable();
            foreach(var column in scrambledColumnsList)
            {
                scrambledColumnsTable.Rows.Add(column);
            }

            var getScramblingScriptParams = new List<SqlParameter>
            {
            new SqlParameter() {ParameterName = SqlParameterStore.constColumnsAndValuesTableParam, SqlDbType = SqlDbType.Structured,
                Value = constantColumnsAndValuesTable, TypeName = ColumnAndValueListDataTable.dbTypeName},
            new SqlParameter() {ParameterName = SqlParameterStore.scrambledColumnsTableParam, SqlDbType = SqlDbType.Structured,
                Value = scrambledColumnsTable, TypeName = ColumnListDataTable.dbTypeName},
            new SqlParameter(SqlParameterStore.sqlToGetConstantTypesParam, sqlToGetConstantTypes),
            new SqlParameter(SqlParameterStore.sqlToGetScrambledTypesParam, sqlToGetScrambledTypes),
            new SqlParameter(SqlParameterStore.dbNameParam, tableInfo.DbName),
            new SqlParameter(SqlParameterStore.schemaNameParam, tableInfo.SchemaName),
            new SqlParameter(SqlParameterStore.tableNameParam, tableInfo.TableName),
            new SqlParameter(SqlParameterStore.whereParam, whereClause)
            };

            try
            {
                SqlHelper.ExecuteNonQuery(tableInfo.DbConnectionString, getScramblingScriptParams, Resources.Scramble);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
