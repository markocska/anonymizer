using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.TableInfo.Common
{
    public class MappedColumnPair
    {
        public string SourceConnectionString { get; }
        public string SourceTableNameWithSchema { get; }
        public string DestinationConnectionString { get; }
        public string DestinationTableNameWithSchema { get; }
        public ColumnPair MappedColumns { get; }
    }
}
