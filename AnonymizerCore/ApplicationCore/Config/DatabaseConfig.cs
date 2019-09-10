using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.Config
{

    public class DatabaseConfig
    {
        public string Version { get; set; }
        public string ConnectionString { get; set; }
        public List<TableConfig> Tables { get; set; }

    }
}
