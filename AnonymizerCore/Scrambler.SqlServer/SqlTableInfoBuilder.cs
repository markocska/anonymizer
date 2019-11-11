using Microsoft.Extensions.Logging;
using Scrambler.Config;
using Scrambler.DatabaseServices.ColumnTypes;
using Scrambler.DatabaseServices.PrimaryKeys;
using Scrambler.SqlServer.Utilities;
using Scrambler.TableInfo.Abstract;
using Scrambler.Utilities;
using Scrambler.Validators;
using Scrambler.Validators.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Scrambler.TableInfo
{
    public class SqlTableInfoBuilder : TableInfoBuilder
    {

        public SqlTableInfoBuilder(DatabaseConfig dbConfig, TableConfig tableConfig, IConfigValidator configValidator, IWhereConditionValidator whereConditionValidator,
            ILinkedServerValidator linkedServerValidator, IColumnTypeManager columnTypeManager, IPrimaryKeyManager primaryKeyManager) :
            base(dbConfig, tableConfig, configValidator, whereConditionValidator, linkedServerValidator ,columnTypeManager, primaryKeyManager)
        {

        }

        protected override string ParseDataSource(string connectionString)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(DatabaseConfig.ConnectionString);

            return ParameterNameHelper.AddParenthesises(connectionStringBuilder.InitialCatalog);
        }

        protected override (string schemaName, string tableName) ParseSchemaAndTableName(string schemaAndTableName)
        {
            var tableAndSchemaName = schemaAndTableName.Split('.');

            return (schemaName: ParameterNameHelper.AddParenthesises(tableAndSchemaName[1]), tableName: ParameterNameHelper.AddParenthesises(tableAndSchemaName[2]));
        }

       
        protected override TableConfig NormalizeTableConfigParameters(TableConfig tableConfig)
        {

            var normalizedTableConfig = new TableConfig
            {
                ConstantColumns = tableConfig.ConstantColumns?.Select(c =>
                new ConstantColumnConfig { Name = ParameterNameHelper.RemoveParenthesises(c.Name), Value = c.Value }).ToList()
                ?? new List<ConstantColumnConfig>(),

                ScrambledColumns = tableConfig.ScrambledColumns?.Select(c =>
                new ScrambledColumnConfig { Name = ParameterNameHelper.RemoveParenthesises(c.Name) }).ToList()
                ?? new List<ScrambledColumnConfig>(),

                PairedColumnsInsideTable = tableConfig.PairedColumnsInsideTable?
                .Select(l => ParameterNameHelper.RemoveParenthesisesFromStringList(l)).ToList()
                ?? new List<List<string>>(),

                PairedColumnsOutsideTable = tableConfig.PairedColumnsOutsideTable?
                .Select(p =>
                    new PairedColumnsOutsideTableConfig
                    {
                        ColumnMapping = p.ColumnMapping.Select(l => ParameterNameHelper.AddParenthesisesToStrList(l)).ToList(),
                        SourceDestMapping = p.SourceDestMapping.Select(s =>
                            new SourceDestMappingStepConfig
                            {
                                DestinationLinkedInstance = ParameterNameHelper.AddParenthesises(s.DestinationLinkedInstance),
                                DestinationConnectionString = s.DestinationConnectionString,
                                DestinationFullTableName = ParameterNameHelper.AddParenthesisToFullTableName(s.DestinationFullTableName),
                                ForeignKeyMapping = s.ForeignKeyMapping.Select(l => ParameterNameHelper.AddParenthesisesToStrList(l)).ToList()
                            }).ToList()
                    }).ToList()
                    ?? new List<PairedColumnsOutsideTableConfig>(),

                FullTableName = ParameterNameHelper.AddParenthesisToFullTableName(tableConfig.FullTableName),
                Where = tableConfig.Where
            };

            return normalizedTableConfig;
        }
    }
}
