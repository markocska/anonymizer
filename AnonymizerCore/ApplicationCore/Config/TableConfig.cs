using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.Config
{
    public class TableConfig
    {
        public string FullTableName { get; set; }
        public List<ScrambledColumnConfig> ScrambledColumns { get; set; }
        public List<ConstantColumnConfig> ConstantColumns { get; set; }
        public List<List<string>> PairedColumnsInsideTable { get; set; }
        public List<PairedColumnsOutsideTableConfig> PairedColumnsOutsideTable { get; set; }
    }
}
