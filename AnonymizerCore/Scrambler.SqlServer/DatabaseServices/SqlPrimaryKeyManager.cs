using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;

namespace Scrambler.DatabaseServices.PrimaryKeys
{
    public class SqlPrimaryKeyManager : IPrimaryKeyManager
    {
        public List<string> GetPrimaryKeys(string connectionString, string tableNameWithSchema)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            using (var adapter = new SqlDataAdapter("select * from " + tableNameWithSchema + ";", sqlConnection))
            using (var tableMetadata = new DataTable(tableNameWithSchema))
            {
                return adapter
                       .FillSchema(tableMetadata, SchemaType.Mapped).PrimaryKey.Select(c => c.ColumnName ).ToList();
            }
        }
    }
}
