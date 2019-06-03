using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.SqlScripts.SqlServer
{
    public partial class TurnOnOffIndexes
    {
        public string FullTableName { get; set; }
        public List<string> IndexNames { get; set; }
        public bool Enable { get; set; }

        public TurnOnOffIndexes(string fullTableName, List<string> indexNames, bool enable)
        {
            FullTableName = fullTableName;
            Enable = enable;
            IndexNames = indexNames;
        }
    }
}
