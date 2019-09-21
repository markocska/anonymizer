using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Scrambler.TableInfo.Interfaces;
using Scrambler.Utilities;

namespace Scrambler.DatabaseServices.Scrambling
{
    public abstract class TableScramblingService : ITableScramblingService
    {
        protected ILogger _logger;
        protected IQueryHelper _queryHelper;

        public TableScramblingService(ILogger<TableScramblingService> logger, IQueryHelper queryHelper)
        {
            _logger = logger;
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
                    _logger.LogError($"Couldn't scramble table {tableInfo.FullTableName}. Connection string: {tableInfo.DbConnectionString}. " +
                        $"Reason: {ex.Message}", ex);
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Couldn't scramble table {tableInfo.FullTableName}. Connection string: {tableInfo.DbConnectionString}. " +
                       $"Reason: {ex.Message}", ex);
                }
            }
        }

        public abstract string GenerateScramblingScript(ITableInfo tableInfo);
        public abstract string ScrambleTable(ITableInfo tableInfo, string scramblingScript);
    }
}
