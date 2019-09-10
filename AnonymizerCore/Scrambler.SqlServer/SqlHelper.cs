using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Scrambler.Utilities.QueryHelpers
{
    public static class SqlHelper
    {
        #region Synchronous Methods
        public static void ExecuteNonQuery(string connectionString, List<SqlParameter> cmdParameters, string cmdStr)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(cmdStr, conn)
                {
                    CommandTimeout = 0,
                    CommandType = CommandType.Text
                };
                if (cmdParameters != null)
                {
                    cmd.Parameters.AddRange(cmdParameters.ToArray());
                }
                cmd.ExecuteNonQuery();
            }
        }

        public static DataTable ExecuteQuery(string connectionString, List<SqlParameter> cmdParameters, string cmdStr)
        {
            DataTable Results = new DataTable();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(cmdStr, conn)
                {
                    CommandTimeout = 0,
                    CommandType = System.Data.CommandType.Text
                };
                if (cmdParameters != null)
                {
                    cmd.Parameters.AddRange(cmdParameters.ToArray());
                }
                using (var Reader = cmd.ExecuteReader())
                {
                    Results.Load(Reader);
                }
            }

            return Results;
        }

        #endregion
    }
}
