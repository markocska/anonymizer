using Anonymizer.Properties;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anonymizer
{
    public static class DbInitializer
    {
        public static void CreateDbTypes(string connectionString, string db, string schema)
        { 
            SqlHelper.ExecuteNonQuery(connectionString, new List<SqlParameter>(), Resources.Create_types);
        }

    }
}
