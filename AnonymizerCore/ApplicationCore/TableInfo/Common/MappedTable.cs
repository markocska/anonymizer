using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.TableInfo.Common
{
    public class MappedTable
    {
        public List<ColumnPair> SourceDestPairedColumnsOutside { get; set; }
        public List<MappedColumnPair> MappedColumnPairsOutside { get; set; }
    }
}
