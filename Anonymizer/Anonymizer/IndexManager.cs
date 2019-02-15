using Anonymizer.ConstantStores;
using Anonymizer.DbObjects;
using Anonymizer.Properties;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Anonymizer
{
    public static class IndexManager
    {
        public static bool TurnIndexesOnOff(TableInfo tableInfo, bool enable)
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
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
