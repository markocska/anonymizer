using Anonymizer.ConstantStores;
using Anonymizer.DbObjects;
using Anonymizer.DbTypes;
using Anonymizer.Properties;
using Anonymizer.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Anonymizer.DbServices
{
    public static class ColumnTypeManager
    {
        public static string GenerateColumnTypeSqlQuery(ITableInfo tableInfo, List<string> columnNames)
        {
            var columnNameTable = new ColumnListDataTable();
            foreach (var columnName in columnNames)
            {
                columnNameTable.Rows.Add(columnName);
            }

            var getColumnTypesQueryParams = new SqlParameter[]
           {
                 new SqlParameter() {ParameterName = SqlParameterStore.columnsTableParam, SqlDbType = SqlDbType.Structured,
                     Value = columnNameTable, TypeName = ColumnListDataTable.dbTypeName},
                 new SqlParameter(SqlParameterStore.dbNameParam, tableInfo.DbName),
                 new SqlParameter(SqlParameterStore.schemaNameParam, tableInfo.SchemaName),
                 new SqlParameter(SqlParameterStore.tableNameParam, tableInfo.TableName)
           };

            var resultTable = new DataTable();
            resultTable = SqlHelper.ExecuteQuery(tableInfo.DbConnectionString, getColumnTypesQueryParams, Resources.Get_Column_Types);

            var resultStr = resultTable.Rows[0][0] as string;

            if (resultStr == null)
            {
                throw new Exception("The result query returned a null value.");
            }

            return resultStr;
        }
    }
}
