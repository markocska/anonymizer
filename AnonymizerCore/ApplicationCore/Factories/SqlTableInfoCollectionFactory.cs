using ApplicationCore.Config;
using ApplicationCore.DatabaseServices.ColumnTypes;
using ApplicationCore.DatabaseServices.PrimaryKeys;
using ApplicationCore.TableInfo;
using ApplicationCore.TableInfo.Interfaces;
using ApplicationCore.Validators.ConfigValidators;
using ApplicationCore.Validators.ParameterValidators;

namespace ApplicationCore.Factories
{
    public class SqlTableInfoCollectionFactory : TableInfoCollectionFactory
    {

        public SqlTableInfoCollectionFactory(IConfigValidator configValidator, IParameterValidator parameterValidator, IColumnTypeManager columnTypeManager,
                 IPrimaryKeyManager primaryKeyManager) : base(configValidator, parameterValidator, columnTypeManager, primaryKeyManager)
        {
        }

        protected override ITableInfo CreateTableInfo(DatabaseConfig dbConfig, TableConfig tableConfig, IConfigValidator configValidator,
            IColumnTypeManager columnTypeManager, IPrimaryKeyManager primaryKeyManager)
        {
            return new SqlTableInfoBuilder(dbConfig, tableConfig, configValidator, columnTypeManager, primaryKeyManager).Build();
        }
    }
}
