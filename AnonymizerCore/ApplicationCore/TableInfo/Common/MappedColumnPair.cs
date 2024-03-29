﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.TableInfo.Common
{
    public class MappedColumnPair
    {
        public string SourceConnectionString { get; set; }
        public string SourceFullTableName { get; set; }
        public string SourceInstance { get; set; }
        public string DestinationConnectionString { get; set; }
        public string DestinationFullTableName { get; set; }
        public string DestinationInstance { get; set; }
        public List<ColumnPair> MappedColumns { get; set; }
    }
}
