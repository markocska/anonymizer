using ApplicationCore.TableInfo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.SqlScripts.SqlServer
{
    public partial class ScrambleTable
    {
        private ITableInfo TableInfo { get; set; }

        public ScrambleTable(ITableInfo tableInfo)
        {
            TableInfo = tableInfo;
        }
    }
}
