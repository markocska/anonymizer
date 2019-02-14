using System.Data;

namespace Anonymizer.DbObjects
{
    public class ColumnListDataTable : DataTable
    {
        public ColumnListDataTable()
        {
            Columns.Add("column_name", typeof(string));
        }

        public const string dbTypeName = "dbo.AnonymizerColumnList";

    }
}
