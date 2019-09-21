using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Scrambler.DatabaseServices.ColumnTypes;
using Scrambler.SqlScripts.SqlServer;
using Scrambler.TableInfo;
using Scrambler.TableInfo.Interfaces;
using Scrambler.Utilities;
using Scrambler.Utilities.QueryHelpers;

namespace Scrambler.DatabaseServices.ColumnTypes
{
    public class SqlColumnTypesManager : IColumnTypeManager
    {
        private readonly IQueryHelper _queryHelper;

        public SqlColumnTypesManager(IQueryHelper queryHelper)
        {
            _queryHelper = queryHelper;
        }

        public Dictionary<string, string> GetColumnNamesAndTypes(ITableInfo tableInfo, List<string> columnNames)
        {
            var columnTypesTemplate = new GetColumnTypes(tableInfo.DbName, tableInfo.SchemaName, tableInfo.TableName, columnNames);
            var columnTypesQuery = columnTypesTemplate.TransformText();

            //Console.WriteLine(columnTypesQuery);
            DataTable columnTypesTable;
            try
            {
                columnTypesTable = _queryHelper.ExecuteQueryWithoutParams(tableInfo.DbConnectionString, columnTypesQuery);
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
