using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ApplicationCore.SqlScripts.SqlServer;
using ApplicationCore.TableInfo.Interfaces;
using ApplicationCore.Utilities.QueryHelpersSqlpH;

namespace ApplicationCore.DatabaseServices.Indexes
{
    public class SqlIndexService : IIndexService
    {
        public void TurnOffNonUniqueIndexes(ITableInfo tableInfo)
        {
            if (tableInfo == null) { throw new ArgumentException("The table info parameter can not be null."); };
          
        }
        private List<string> GetEnabledIndexNames(ITableInfo tableInfo)
        {
            return GetEnabledDisabledIndexNames(tableInfo, true);
        }

        private List<string> GetDisabledIndexNames(ITableInfo tableInfo)
        {
            return GetEnabledDisabledIndexNames(tableInfo, false);
        }

        private List<string> GetEnabledDisabledIndexNames(ITableInfo tableInfo, bool isEnabled)
        {
            var indexNamesTemplate = new GetIndexes(tableInfo.DbName,tableInfo.SchemaName, tableInfo.TableName, isEnabled);
            var indexNamesQuery = indexNamesTemplate.TransformText();
            Console.WriteLine(indexNamesQuery);
            DataTable indexNamesTable;
            try
            {
                indexNamesTable = SqlHelper.ExecuteQuery(tableInfo.DbConnectionString, new List<SqlParameter>(), indexNamesQuery);
            }
            catch (Exception ex)
            {
                throw new IndexServiceException("An error happened while trying to get column types.", ex);
            }

            var columnNames = new List<string>();
            foreach (var rowObject in indexNamesTable.Rows)
            {
                var row = (DataRow)rowObject;
                columnNames.Add((string)row[0]);
            }

            return columnNames;
        }
    }
}
