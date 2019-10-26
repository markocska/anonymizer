using Microsoft.Extensions.Logging;
using Scrambler.Config;
using Scrambler.DatabaseServices.ColumnTypes;
using Scrambler.DatabaseServices.PrimaryKeys;
using Scrambler.TableInfo;
using Scrambler.TableInfo.Interfaces;
using Scrambler.Validators;
using Scrambler.Validators.Interfaces;
using System.Linq;

namespace Scrambler.Factories
{
    public class SqlTableInfoCollectionFactory : TableInfoCollectionFactory
    {

        public SqlTableInfoCollectionFactory(IConfigValidator configValidator, IParameterValidator parameterValidator, IColumnTypeManager columnTypeManager,
                 IPrimaryKeyManager primaryKeyManager, IWhereConditionValidator whereConditionValidator,ILinkedServerValidator linkedServerValidator,ILogger<SqlTableInfoCollectionFactory> logger) 
            : base(configValidator, parameterValidator, columnTypeManager, primaryKeyManager, whereConditionValidator ,linkedServerValidator, logger)
        {
        }


        protected override ITableInfo CreateTableInfo(DatabaseConfig dbConfig, TableConfig tableConfig)
        {
            return new SqlTableInfoBuilder(dbConfig, tableConfig, _configValidator, _whereConditionValidator, _linkedServerValidator, _columnTypeManager, _primaryKeyManager, _logger).Build();
        }

        protected override void ConstructFullTableName(TableConfig tableConfig)
        {
            if (tableConfig.PairedColumnsOutsideTable == null) { return; }


            foreach (var outsideConfig in tableConfig.PairedColumnsOutsideTable)
            {
                foreach (var mappingStep in outsideConfig.SourceDestMapping)
                {
                    if (!string.IsNullOrEmpty(mappingStep.DestinationLinkedInstance))
                    {
                        if (mappingStep.DestinationFullTableName.Split('.')[0] == mappingStep.DestinationLinkedInstance)
                        {
                            continue;
                        }
                        mappingStep.DestinationFullTableName = mappingStep.DestinationLinkedInstance + "." + mappingStep.DestinationFullTableName;
                    }
                }
            }
        }
    }
}
