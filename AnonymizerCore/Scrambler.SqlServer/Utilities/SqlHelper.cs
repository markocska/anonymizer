using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Scrambler.Utilities.QueryHelpers
{
    public class SqlHelper : IQueryHelper
    {
        #region Synchronous Methods
        public void ExecuteNonQueryWithoutParams(string connectionString, string cmdStr)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(cmdStr, conn)
                {
                    CommandTimeout = 0,
                    CommandType = CommandType.Text
                };

                cmd.ExecuteNonQuery();
            }
        }

        public DataTable ExecuteQueryWithoutParams(string connectionString, string cmdStr)
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
