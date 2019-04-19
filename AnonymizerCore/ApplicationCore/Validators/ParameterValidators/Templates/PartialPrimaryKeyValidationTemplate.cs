using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Validators.ParameterValidators.Templates
{
    public partial class PrimaryKeyValidationTemplate
    {
        private List<string> ConstantColumns { get; set; }
        private List<string> ScrambledColumns { get; set; }
        private string Database { get; set; }
        private string Schema { get; set; }
        private string Table { get; set; }

        public PrimaryKeyValidationTemplate(List<string> constantColumns, List<string> scrambledColumns,
            string database, string schema, string table)
        {
            ConstantColumns = constantColumns;
            ScrambledColumns = scrambledColumns;
            Database = database;
            Schema = schema;
            Table = table;
        }
    }
}
