using Microsoft.Extensions.Logging;
using Scrambler.Config;
using Scrambler.DatabaseServices.ColumnTypes;
using Scrambler.DatabaseServices.PrimaryKeys;
using Scrambler.TableInfo;
using Scrambler.TableInfo.Interfaces;
using Scrambler.Validators;

namespace Scrambler.Factories
{
    public class SqlTableInfoCollectionFactory : TableInfoCollectionFactory
    {

        public SqlTableInfoCollectionFactory(IConfigValidator configValidator, IParameterValidator parameterValidator, IColumnTypeManager columnTypeManager,
                 IPrimaryKeyManager primaryKeyManager, ILogger<SqlTableInfoCollectionFactory> logger) 
            : base(configValidator, parameterValidator, columnTypeManager, primaryKeyManager, logger)
        {
        }

        protected override ITableInfo CreateTableInfo(DatabaseConfig dbConfig, TableConfig tableConfig, IConfigValidator configValidator,
            IColumnTypeManager columnTypeManager, IPrimaryKeyManager primaryKeyManager)
        {
            return new SqlTableInfoBuilder(dbConfig, tableConfig, configValidator, columnTypeManager, primaryKeyManager, _logger).Build();
        }
    }
}
