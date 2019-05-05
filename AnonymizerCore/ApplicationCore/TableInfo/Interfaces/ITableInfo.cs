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
        Dictionary<string, string> ScrambledColumns { get; set; }
        List<ColumnAndTypeAndValue> ConstantColumnsAndValues { get; set; }
        List<ColumnPair> PairedColumnsInside { get; set; }
        ColumnPair SourceDestPairedColumnsOutside { get; set; }
        List<MappedColumnPair> mappedColumnPairsOutside { get; set; }
        string WhereClause { get; set; }
        string FullTableName { get; }
    }
}
