using MySql.Data.MySqlClient;
using Scrambler.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Scrambler.MySql.Utilities
{
    public class MySqlHelper : IQueryHelper
    {
        #region Synchronous Methods
        public void ExecuteNonQueryWithoutParams(string connectionString, string cmdStr)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(cmdStr, conn)
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
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(cmdStr, conn)
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
