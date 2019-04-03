using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Config
{
    public enum DatabaseType
    {
        MSSQL, Oracle, MySql
    }

    public class DatabaseConfig
    {
        public DatabaseType Type { get; set; }
        public string Version { get; set; }
        public string ConnectionString { get; set; }
        public List<TableConfig> Tables { get; set; }

    }
}
