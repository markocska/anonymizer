using Scrambler.TableInfo;
using Scrambler.TableInfo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.DatabaseServices.ColumnTypes
{
    public interface IColumnTypeManager
    {
        Dictionary<string, string> GetColumnNamesAndTypes(ITableInfo tableInfo, List<string> columnNames);
    }
}
