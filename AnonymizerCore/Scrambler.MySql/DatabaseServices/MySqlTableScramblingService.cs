using Scrambler.DatabaseServices.Scrambling;
using Scrambler.MySql.SqlScripts;
using Scrambler.TableInfo.Interfaces;
using Scrambler.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.MySql.DatabaseServices
{
    public class MySqlTableScramblingService : TableScramblingService
    {
        public MySqlTableScramblingService(IQueryHelper queryHelper) : base(queryHelper)
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
