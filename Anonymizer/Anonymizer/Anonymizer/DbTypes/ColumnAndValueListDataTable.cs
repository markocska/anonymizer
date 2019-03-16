using System.Data;

namespace Anonymizer.DbTypes
{
    public class ColumnAndValueListDataTable : DataTable
    {
        public ColumnAndValueListDataTable()
        {
            Columns.Add("column_name",typeof(string));
            Columns.Add("column_value", typeof(string));
        }

        public const string dbTypeName = "dbo.AnonymizerColumnAndValueList";
    }
}
