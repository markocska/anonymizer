using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.TableInfo
{
    public class TableMappingRouteElement
    {
        public string ConnectionString { get; set; }
        public string TableNameWithSchema { get; set; }
        public List<ColumnPair> ForeignKeyMapping { get; set; }
    }
}
