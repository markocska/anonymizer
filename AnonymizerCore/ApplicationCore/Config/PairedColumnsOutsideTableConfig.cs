using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Config
{
    public class PairedColumnsOutsideTableConfig
    {
        public List<List<string>> ColumnMapping { get; set; }
        public List<SourceDestMappingStepConfig> SourceDestMapping { get; set; }
    }
}
