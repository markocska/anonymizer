﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Config
{
    public class TableConfig
    {
        public string NameWithSchema { get; set; }
        public List<ScrambledColumnConfig> ScrambledColumns { get; set; }
        public List<ConstantColumnConfig> ConstantColumns { get; set; }
        public List<List<string>> PairedColumnsInsideTable { get; set; }
        public List<PairedColumnsOutsideTableConfig> PairedColumnsOutsideTable { get; set; }
    }
}