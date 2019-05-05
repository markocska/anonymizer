using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ApplicationCore.SqlScripts.SqlServer;
using ApplicationCore.TableInfo;
using ApplicationCore.TableInfo.Interfaces;
using ApplicationCore.Utilities.QueryHelpersSqlpH;

namespace ApplicationCore.DatabaseServices.ColumnTypes
{
    public class SqlColumnTypesManager : IColumnTypeManager
    {
        public Dictionary<string, string> GetColumnNamesAndTypes(ITableInfo tableInfo, List<string> columnNames)
        {
            var columnTypesTemplate = new GetColumnTypes(tableInfo.DbName, tableInfo.SchemaName, tableInfo.TableName, columnNames);
            var columnTypesQuery = columnTypesTemplate.TransformText();

            DataTable columnTypesTable;
            try
            {
                columnTypesTable = SqlHelper.ExecuteQuery(tableInfo.DbConnectionString, new List<SqlParameter>(), columnTypesQuery);
            }
            catch (Exception ex)
            {
                throw new ColumnTypesException("An error happened while trying to get column types.", ex);
            }

            var columnsAndTypes = new Dictionary<string, string>();
            foreach (var rowObject in columnTypesTable.Rows)
            {
                var row = (DataRow) rowObject;
                columnsAndTypes.Add((string)row[GetColumnTypes.columnName],
                                                      (string)row[GetColumnTypes.columnType]);
            }

            return columnsAndTypes;
        } 
    }
}
