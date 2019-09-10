using Scrambler.SqlScripts.SqlServer;
using Scrambler.TableInfo.Interfaces;
using Scrambler.Utilities.QueryHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Scrambler.DatabaseServices.Indexes
{
    public class SqlIndexService : IIndexService
    {
        public void TurnOffIndexes(ITableInfo tableInfo)
        {
            EnableDisableIndexes(tableInfo, false);
        }

        public void TurnOnIndexes(ITableInfo tableInfo)
        {
            EnableDisableIndexes(tableInfo, true);
        }

        private void EnableDisableIndexes(ITableInfo tableInfo, bool enable)
        {
            if (tableInfo == null) { throw new ArgumentException("The table info parameter can not be null."); };

            List<string> indexNames;
            if (enable)
            {
                indexNames = GetDisabledIndexNames(tableInfo);
            }
            else
            {
                indexNames = GetEnabledIndexNames(tableInfo);
            }

            var indexOnOffTemplate = new TurnOnOffIndexes(tableInfo.FullTableName, indexNames, enable);
            var indexOnOffQuery = indexOnOffTemplate.TransformText();
            Console.WriteLine(indexOnOffQuery);
            try
            {
                SqlHelper.ExecuteNonQuery(tableInfo.DbConnectionString, new List<SqlParameter>(), indexOnOffQuery);
            }
            catch(Exception ex)
            {
                var enableDisableStr = enable ? "enable" : "disable";
                throw new IndexServiceException($"Error while trying to {enableDisableStr} indexes.",ex);
            }

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
            var indexNamesTemplate = new GetIndexes(tableInfo.DbName, tableInfo.SchemaName, tableInfo.TableName, isEnabled);
            var indexNamesQuery = indexNamesTemplate.TransformText();
            Console.WriteLine(indexNamesQuery);
            DataTable indexNamesTable;
            try
            {
                indexNamesTable = SqlHelper.ExecuteQuery(tableInfo.DbConnectionString, new List<SqlParameter>(), indexNamesQuery);
            }
            catch (Exception ex)
            {
                throw new IndexServiceException("An error happened while trying to get index names.", ex);
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
