using System;
using System.Collections.Generic;
using System.Text;
using Scrambler.SqlScripts.SqlServer;
using Scrambler.TableInfo.Interfaces;
using Scrambler.Utilities.QueryHelpers;

namespace Scrambler.DatabaseServices.Scrambling
{
    public class SqlTableScramblingService : TableScramblingService
    {
        public override string GenerateScramblingScript(ITableInfo tableInfo)
        {
            var scramblingTemplate = new ScrambleTable(tableInfo);
            return scramblingTemplate.TransformText();
        }

        public override string ScrambleTable(ITableInfo tableInfo, string scramblingScript)
        {
            SqlHelper.ExecuteNonQuery(tableInfo.DbConnectionString, null, scramblingScript);
            return "test";
        }
    }
}
