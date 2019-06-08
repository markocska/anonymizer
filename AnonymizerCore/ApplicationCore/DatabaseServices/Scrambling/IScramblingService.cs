using ApplicationCore.TableInfo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DatabaseServices.Scrambling
{
    public interface IScramblingService
    {
        void ScrambleTables(List<ITableInfo> tableInfos);
    }
}
