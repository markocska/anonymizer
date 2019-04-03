using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Config
{
    public class PairedColumnsOutsideTableConfig
    {
        public string ScrambledColumnInSourceTable { get; set; }
        public string ScrambledColumnInTargetTable { get; set; }
        public string SourceConnectionString { get; set; }
        public string SourceTableNameWithSchema { get; set; }
        public List<SourceDestMappingStepConfig> SourceDestMapping { get; set; }
    }
}
