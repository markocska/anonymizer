using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.SqlScripts.SqlServer;
using ApplicationCore.TableInfo.Interfaces;

namespace ApplicationCore.DatabaseServices.Scrambling
{
    public class SqlScramblingService : ScramblingService
    {
        public override string GenerateScramblingScript(ITableInfo tableInfo)
        {
            var scramblingTemplate = new ScrambleTable(tableInfo);
            return scramblingTemplate.TransformText();
        }

        public override string ScrambleTable(ITableInfo tableInfo, string scramblingScript)
        {
            return "test";
        }
    }
}
