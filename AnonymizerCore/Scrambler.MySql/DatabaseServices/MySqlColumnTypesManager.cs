using Scrambler.DatabaseServices.ColumnTypes;
using Scrambler.MySql.SqlScripts.MySql;
using Scrambler.TableInfo.Interfaces;
using Scrambler.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Scrambler.MySql.DatabaseServices
{
    public class MySqlColumnTypesManager : IColumnTypeManager
    {
        private readonly IQueryHelper _queryHelper;

        public MySqlColumnTypesManager(IQueryHelper queryHelper)
        {
            _queryHelper = queryHelper;
        }

        public Dictionary<string, string> GetColumnNamesAndTypes(ITableInfo tableInfo, List<string> columnNames)
        {
            var columnTypesTemplate = new GetColumnTypes(ParameterNameHelper.RemoveQuotationMarks(tableInfo.SchemaName), 
                ParameterNameHelper.RemoveQuotationMarks(tableInfo.TableName), columnNames);
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
                var row = (DataRow)rowObject;
                columnsAndTypes.Add((string)row[GetColumnTypes.columnName],
                                                      (string)row[GetColumnTypes.columnType]);
            }

            return columnsAndTypes;
        }
    }
}
