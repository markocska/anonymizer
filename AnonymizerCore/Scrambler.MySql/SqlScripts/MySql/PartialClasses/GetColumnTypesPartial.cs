using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.MySql.SqlScripts.MySql
{
    public partial class GetColumnTypes
    {
        private string Schema { get; set; }
        private string Table { get; set; }
        private List<string> Columns { get; set; }
        public GetColumnTypes(string schema, string table, List<string> columns)
        {
            Schema = schema;
            Table = table;
            Columns = columns;
        }

        public const string columnName = "column_name";
        public const string columnType = "column_type";
    }
}
