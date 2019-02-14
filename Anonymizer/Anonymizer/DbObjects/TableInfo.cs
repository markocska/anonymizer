using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anonymizer.DbObjects
{
    public class TableInfo
    {
        public string DbConnectionString { get; set; }
        public string DbName { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }

        public TableInfo(string dbConnectionString, string dbName,
                         string schemaName, string tableName) 
        {
            DbConnectionString = dbConnectionString;
            DbName = dbName;
            SchemaName = schemaName;
            TableName = tableName;
        }
    }
}
