using ApplicationCore.TableInfo.Common;
using System.Collections.Generic;

namespace ApplicationCore.TableInfo.Interfaces
{
    public interface ITableInfo
    {
        string DbConnectionString { get; set; }
        string DbName { get; set; }
        string SchemaName { get; set; }
        string TableName { get; set; }
        Dictionary<string, string> PrimaryKeysAndTypes { get; set; }
        Dictionary<string, string> SoloScrambledColumnsAndTypes { get; set; }
        List<ColumnAndTypeAndValue> ConstantColumnsAndTypesAndValues { get; set; }
        List<Dictionary<string, string>> PairedColumnsInside { get; set; }
        List<MappedTable> MappedTablesOutside { get; set; }
        string WhereClause { get; set; }
        string FullTableName { get; }
    }
}
