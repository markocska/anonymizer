using MySql.Data.MySqlClient;
using Scrambler.DatabaseServices.PrimaryKeys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Scrambler.MySql.DatabaseServices
{
    public class MySqlPrimaryKeyManager : IPrimaryKeyManager
    {
        public List<string> GetPrimaryKeys(string connectionString, string tableNameWithSchema)
        {
            using (var sqlConnection = new MySqlConnection(connectionString))
            using (var adapter = new MySqlDataAdapter("select * from " + tableNameWithSchema + ";", sqlConnection))
            using (var tableMetadata = new DataTable(tableNameWithSchema))
            {
                return adapter
                       .FillSchema(tableMetadata, SchemaType.Mapped).PrimaryKey.Select(c => c.ColumnName).ToList();
            }
        }
    }
}
