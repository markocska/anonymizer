using Anonymizer.Properties;
using Anonymizer.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Anonymizer.DbServices
{
    public static class DbTypeInitializer
    {
        public static void CreateDbTypes(string connectionString)
        { 
            SqlHelper.ExecuteNonQuery(connectionString, new List<SqlParameter>(), Resources.Create_types);
        }

    }
}
