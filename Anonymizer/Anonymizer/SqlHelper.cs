using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anonymizer
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

        public static DataTable ExecuteQuery(string ConnectionString, SqlParameter[] CmdParameters, string CmdStr)
        {
            DataTable Results = new DataTable();
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(CmdStr, conn)
                {
                    CommandTimeout = 0,
                    CommandType = System.Data.CommandType.Text
                };
                if (CmdParameters != null)
                {
                    cmd.Parameters.AddRange(CmdParameters);
                }
                using (var Reader = cmd.ExecuteReader())
                {
                    Results.Load(Reader);
                }
            }

            return Results;
        }

        public static void ScrambleTable(string connectionString, string db, string schema, string table, string whereClause )
        {
            
        }

        #endregion

        #region Async methods
        public static async Task<int> ExecuteNonQueryAsync(string ConnectionString, SqlParameter[] CmdParameters, string CmdStr, CommandType CmdType)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(CmdStr, conn)
                {
                    CommandTimeout = 0,
                    CommandType = CmdType
                };
                if (CmdParameters != null)
                {
                    cmd.Parameters.AddRange(CmdParameters);
                }

                int numberOfRowsAffected = await cmd.ExecuteNonQueryAsync(); ;

                return numberOfRowsAffected;
            }
        }

        public static async Task<int> ExecuteStoredProcedureAsync(string ConnectionString, SqlParameter[] CmdParameters, string ProcedureName)
        {
            int numberOfRowsAffected = await ExecuteNonQueryAsync(ConnectionString, CmdParameters, ProcedureName, System.Data.CommandType.StoredProcedure);

            return numberOfRowsAffected;
        }

        #endregion
    }
}
