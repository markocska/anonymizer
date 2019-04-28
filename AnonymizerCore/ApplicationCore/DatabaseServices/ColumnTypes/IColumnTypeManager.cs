using ApplicationCore.TableInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DatabaseServices.ColumnTypes
{
    public interface IColumnTypeManager
    {
        List<ColumnAndType> GetColumnTypes(ITableInfo tableInfo, List<string> columnNames);
    }
}
