using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Config
{
    public class SourceDestMappingStepConfig
    {
        public string ConnectionString { get; set; }
        public string TableNameWithSchema { get; set; }
        public List<List<string>> ForeignKeyMapping { get; set; }
    }
}
