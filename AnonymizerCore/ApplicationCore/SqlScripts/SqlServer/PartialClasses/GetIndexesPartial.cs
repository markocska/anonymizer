using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.SqlScripts.SqlServer
{
    public partial class GetIndexes
    {
        private string Database { get; set; }
        private string Schema { get; set; }
        private string Table { get; set; }
        private bool Enabled { get; set; }
        private List<string> Columns { get; set; }
        public GetIndexes(string database, string schema, string table, bool enabled)
        {
            Database = database;
            Schema = schema;
            Table = table;
            Enabled = enabled;
        }
    }
}
