using Scrambler.TableInfo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.DatabaseServices.Scrambling
{
    public interface ITableScramblingService
    {
        void ScrambleTables(List<ITableInfo> tableInfos);
        string GenerateScramblingScript(ITableInfo tableInfo);
        string ScrambleTable(ITableInfo tableInfo, string scramblingScript);
    }
}
