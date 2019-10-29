using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Scrambler.TableInfo.Interfaces;
using Scrambler.Utilities;
using Serilog;

namespace Scrambler.DatabaseServices.Scrambling
{
    public abstract class TableScramblingService : ITableScramblingService
    {
        protected IQueryHelper _queryHelper;

        public TableScramblingService(IQueryHelper queryHelper)
        {
            _queryHelper = queryHelper;
        }
        public void ScrambleTables(List<ITableInfo> tableInfos)
        {
            foreach (var tableInfo in tableInfos)
            {
                try
                {
                    var scramblingScript = GenerateScramblingScript(tableInfo);
                    ScrambleTable(tableInfo, scramblingScript);
                }
                catch(ScramblingException ex)
                {
                    Log.Error($"Couldn't scramble table {tableInfo.FullTableName}. Connection string: {tableInfo.DbConnectionString}. " +
                        $"Reason: {ex.Message}", ex);
                }
                catch(Exception ex)
                {
                    Log.Error($"Couldn't scramble table {tableInfo.FullTableName}. Connection string: {tableInfo.DbConnectionString}. " +
                       $"Reason: {ex.Message}", ex);
                }
            }
        }

        public abstract string GenerateScramblingScript(ITableInfo tableInfo);
        public abstract string ScrambleTable(ITableInfo tableInfo, string scramblingScript);
    }
}
