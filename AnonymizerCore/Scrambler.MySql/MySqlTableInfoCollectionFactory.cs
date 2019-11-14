using Scrambler.Config;
using Scrambler.DatabaseServices.ColumnTypes;
using Scrambler.DatabaseServices.PrimaryKeys;
using Scrambler.Factories;
using Scrambler.TableInfo.Interfaces;
using Scrambler.Validators;
using Scrambler.Validators.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.MySql
{
    public class MySqlTableInfoCollectionFactory : TableInfoCollectionFactory
    {
        public MySqlTableInfoCollectionFactory(IConfigValidator configValidator, IParameterValidator parameterValidator, IColumnTypeManager columnTypeManager,
         IPrimaryKeyManager primaryKeyManager, IWhereConditionValidator whereConditionValidator, ILinkedServerValidator linkedServerValidator)
        : base(configValidator, parameterValidator, columnTypeManager, primaryKeyManager, whereConditionValidator, linkedServerValidator)
        {
        }

        protected override ITableInfo CreateTableInfo(DatabaseConfig dbConfig, TableConfig tableConfig)
        {
            return new MySqlTableInfoBuilder(dbConfig, tableConfig, _configValidator, _whereConditionValidator, _linkedServerValidator, _columnTypeManager, _primaryKeyManager).Build();
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
