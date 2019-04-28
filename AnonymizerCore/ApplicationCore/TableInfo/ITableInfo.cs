using System.Collections.Generic;

namespace ApplicationCore.TableInfo
{
    public interface ITableInfo
    {
        string DbConnectionString { get; set; }
        string DbName { get; set; }
        string SchemaName { get; set; }
        string TableName { get; set; }
        List<string> ScrambledColumns { get; set; }
        Dictionary<string, string> ConstantColumnsAndValues { get; set; }
        List<ColumnPair> PairedColumnsInside { get; set; }
        ColumnPair SourceDestPairedColumnsOutside { get; set; }
        List<MappedColumnPair> mappedColumnPairsOutside { get; set; }
        string SqlToGetScrambledColumnTypes { get; set; }
        string SqlToGetConstantColumnTypes { get; set; }
        string WhereClause { get; set; }
        string FullTableName { get; }
    }
}
