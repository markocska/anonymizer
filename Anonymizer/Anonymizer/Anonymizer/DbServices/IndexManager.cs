using Anonymizer.ConstantStores;
using Anonymizer.DbObjects;
using Anonymizer.Properties;
using Anonymizer.Utilities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Anonymizer.DbServices
{
    public static class IndexManager
    {
        private static ILogger logger;

        static IndexManager()
        {
            logger = Serilog.Log.ForContext(typeof(IndexManager));
        }

        public static void TurnIndexesOnOff(ITableInfo tableInfo, bool enable)
        {
            var checkingQueryParams = new List<SqlParameter>
                 {
                    new SqlParameter(SqlParameterStore.dbNameParam, tableInfo.DbName),
                    new SqlParameter(SqlParameterStore.schemaNameParam, tableInfo.SchemaName),
                    new SqlParameter(SqlParameterStore.tableNameParam, tableInfo.TableName),
                    new SqlParameter(SqlParameterStore.enableParam, enable)
                 };

            try
            {
                SqlHelper.ExecuteNonQuery(tableInfo.DbConnectionString, checkingQueryParams, Resources.TurnOnOff_Non_PrkeyUniqueClust_Indexes);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
