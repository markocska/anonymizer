using ApplicationCore.TableInfo;
using ApplicationCore.TableInfo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DatabaseServices.ColumnTypes
{
    public interface IColumnTypeManager
    {
        Dictionary<string, string> GetColumnNamesAndTypes(ITableInfo tableInfo, List<string> columnNames);
    }
}
