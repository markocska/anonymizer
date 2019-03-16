using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anonymizer.DbObjects
{
    public interface ITableInfo
    {
        string DbConnectionString { get; set; }
        string DbName { get; set; }
        string SchemaName { get; set; }
        string TableName { get; set; }
        List<string> ScrambledColumns { get; set; }
        Dictionary<string, string> ConstantColumnsAndValues { get; set; }
        string SqlToGetScrambledColumnTypes { get; set; }
        string SqlToGetConstantColumnTypes { get; set; }
        string WhereClause { get; set; }
        string FullTableName { get; }
    }
}
