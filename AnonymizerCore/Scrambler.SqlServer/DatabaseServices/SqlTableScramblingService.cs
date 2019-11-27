using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Scrambler.SqlServer.SqlScripts;
using Scrambler.TableInfo.Interfaces;
using Scrambler.Utilities;
using Scrambler.Utilities.QueryHelpers;

namespace Scrambler.DatabaseServices.Scrambling
{
    public class SqlTableScramblingService : TableScramblingService
    {

        public SqlTableScramblingService(IQueryHelper queryHelper) : base(queryHelper)
        {

        }

        public override string GenerateScramblingScript(ITableInfo tableInfo)
        {
            var scramblingTemplate = new ScrambleTable(tableInfo);
            return scramblingTemplate.TransformText();
        }

        public override string ScrambleTable(ITableInfo tableInfo, string scramblingScript)
        {
            _queryHelper.ExecuteNonQueryWithoutParams(tableInfo.DbConnectionString, scramblingScript);
            return "test";
        }
    }
}
