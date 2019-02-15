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
    public static class ColumnTypeManager
    {
        public static string GenerateColumnTypeSqlQuery(TableInfo tableInfo, List<string> columnNames) 
        {
            var columnNameTable = new ColumnListDataTable();
            foreach(var columnName in columnNames)
            {
                columnNameTable.Rows.Add(columnName);
            }

            var getColumnTypesQueryParams = new SqlParameter[]
           {
                 new SqlParameter() {ParameterName = "@columns_table", SqlDbType = SqlDbType.Structured, Value = columnNameTable, TypeName = "dbo.AnonymizerColumnList"},
                 new SqlParameter("@db_value", "cnfs_hun"),
                 new SqlParameter("@schema_value", "dbo"),
                 new SqlParameter("@table_value", "agreement_table")
           };

            try
            {
                var resultTable = new DataTable();
                resultTable = SqlHelper.ExecuteQuery(tableInfo.DbConnectionString, getColumnTypesQueryParams, Resources.Get_Column_Types);

                var resultStr = resultTable.Rows[0][0] as string;

                if (resultStr == null)
                {
                    throw new Exception("The result the query returned is not of a string type.");
                }

                return resultStr;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
