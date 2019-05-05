using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.TableInfo.Common
{
    public class MappedColumnPair
    {
        public string SourceConnectionString { get; set; }
        public string SourceTableNameWithSchema { get; set; }
        public string DestinationConnectionString { get; set; }
        public string DestinationTableNameWithSchema { get; set; }
        public List<ColumnPair> MappedColumns { get; set; }
    }
}
