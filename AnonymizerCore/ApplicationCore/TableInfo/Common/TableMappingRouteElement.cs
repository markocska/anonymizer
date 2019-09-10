using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.TableInfo.Common
{
    public class TableMappingRouteElement
    {
        public string ConnectionString { get; set; }
        public string TableNameWithSchema { get; set; }
        public List<ColumnPair> ForeignKeyMapping { get; set; }
    }
}
