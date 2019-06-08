using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.TableInfo.Interfaces;
using Serilog;

namespace ApplicationCore.DatabaseServices.Scrambling
{
    public abstract class ScramblingService : IScramblingService
    {
        protected ILogger _logger;
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
                    _logger.Error($"Couldn't scramble table {tableInfo.FullTableName}. Connection string: {tableInfo.DbConnectionString}. " +
                        $"Reason: {ex.Message}", ex);
                }
                catch(Exception ex)
                {
                    _logger.Error($"Couldn't scramble table {tableInfo.FullTableName}. Connection string: {tableInfo.DbConnectionString}. " +
                       $"Reason: {ex.Message}", ex);
                }
            }
        }

        public abstract string GenerateScramblingScript(ITableInfo tableInfo);
        public abstract string ScrambleTable(ITableInfo tableInfo, string scramblingScript);
    }
}
