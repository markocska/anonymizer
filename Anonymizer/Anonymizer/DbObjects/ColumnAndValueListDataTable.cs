using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anonymizer.DbObjects
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
