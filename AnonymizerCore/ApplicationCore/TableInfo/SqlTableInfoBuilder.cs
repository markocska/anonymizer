using ApplicationCore.Config;
using ApplicationCore.DatabaseServices.ColumnTypes;
using ApplicationCore.TableInfo.Abstract;
using ApplicationCore.Validators.ConfigValidators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ApplicationCore.TableInfo
{
    public class SqlTableInfoBuilder : TableInfoBuilder
    {

        public SqlTableInfoBuilder(DatabaseConfig dbConfig, TableConfig tableConfig, IConfigValidator configValidator, IColumnTypeManager columnTypeManager):
            base(dbConfig, tableConfig, configValidator, columnTypeManager)
        {

        }

        protected override string ParseDataSource(string connectionString)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(DatabaseConfig.ConnectionString);

            return connectionStringBuilder.InitialCatalog;
        }

        protected override (string schemaName, string tableName) ParseSchemaAndTableName(string schemaAndTableName)
        {
            var tableAndSchemaName = schemaAndTableName.Split('.');

            return (schemaName: tableAndSchemaName[0], tableName: tableAndSchemaName[1]);
        }
    }
}
