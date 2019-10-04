using Microsoft.Extensions.Logging;
using Scrambler.Config;
using Scrambler.DatabaseServices.ColumnTypes;
using Scrambler.DatabaseServices.PrimaryKeys;
using Scrambler.TableInfo;
using Scrambler.TableInfo.Interfaces;
using Scrambler.Validators;
using Scrambler.Validators.Interfaces;

namespace Scrambler.Factories
{
    public class SqlTableInfoCollectionFactory : TableInfoCollectionFactory
    {

        public SqlTableInfoCollectionFactory(IConfigValidator configValidator, IParameterValidator parameterValidator, IColumnTypeManager columnTypeManager,
                 IPrimaryKeyManager primaryKeyManager, IWhereConditionValidator whereConditionValidator,ILogger<SqlTableInfoCollectionFactory> logger) 
            : base(configValidator, parameterValidator, columnTypeManager, primaryKeyManager, whereConditionValidator ,logger)
        {
        }

        protected override ITableInfo CreateTableInfo(DatabaseConfig dbConfig, TableConfig tableConfig)
        {
            return new SqlTableInfoBuilder(dbConfig, tableConfig, _configValidator, _whereConditionValidator, _columnTypeManager, _primaryKeyManager, _logger).Build();
        }
    }
}
