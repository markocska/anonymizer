using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anonymizer.ConstantStores
{
    public static class SqlParameterStore
    {
        public const string dbNameParam  = "@db_value";

        public const string schemaNameParam = "@schema_value";

        public const string tableNameParam = "@table_value";

        public const string columnsTableParam = "@columns_table";

        public const string constColumnsAndValuesTableParam = "@const_columns_and_values_table";

        public const string constColumnsTableParam = "@const_columns_table";

        public const string scrambledColumnsTableParam = "@scrambled_columns_table";

        public const string sqlToGetConstantTypesParam = "@sql_to_get_constant_types_value";

        public const string sqlToGetScrambledTypesParam = "@sql_to_get_scrambled_types_value";

        public const string whereParam = "@where_value";

        public const string enableParam = "@enable_value";
    }
}
