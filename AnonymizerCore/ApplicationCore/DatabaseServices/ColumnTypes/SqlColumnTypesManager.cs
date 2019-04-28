using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.TableInfo;

namespace ApplicationCore.DatabaseServices.ColumnTypes
{
    public class SqlColumnTypesManager : IColumnTypeManager
    {
        public List<ColumnAndType> GetColumnTypes(ITableInfo tableInfo, List<string> columnNames)
        {
            throw new NotImplementedException();
        }
    }
}
