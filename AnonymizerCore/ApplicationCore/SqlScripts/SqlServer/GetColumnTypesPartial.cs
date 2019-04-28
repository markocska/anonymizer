using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.SqlScripts.SqlServer
{
    partial class GetColumnTypes
    {
        private string Database { get; set; }
        private string Schema { get; set; }
        private string Table { get; set; }
        private List<string> Columns { get; set; }
        public GetColumnTypes(string database, string schema, string table, List<string> columns)
        {
            Database = database;
            Schema = schema;
            Table = table;
            Columns = columns;
        }
    }
}
