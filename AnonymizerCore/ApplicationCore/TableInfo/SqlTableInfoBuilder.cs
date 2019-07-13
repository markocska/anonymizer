using ApplicationCore.Config;
using ApplicationCore.DatabaseServices.ColumnTypes;
using ApplicationCore.DatabaseServices.PrimaryKeys;
using ApplicationCore.TableInfo.Abstract;
using ApplicationCore.Utilities;
using ApplicationCore.Validators.ConfigValidators;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ApplicationCore.TableInfo
{
    public class SqlTableInfoBuilder : TableInfoBuilder
    {

        public SqlTableInfoBuilder(DatabaseConfig dbConfig, TableConfig tableConfig, IConfigValidator configValidator, IColumnTypeManager columnTypeManager,
            IPrimaryKeyManager primaryKeyManager) :
            base(dbConfig, tableConfig, configValidator, columnTypeManager, primaryKeyManager)
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
            var normalizedTableConfig = new TableConfig();


            normalizedTableConfig.ConstantColumns = tableConfig.ConstantColumns?.Select(c =>
            new ConstantColumnConfig { Name = ParameterNameHelper.RemoveParenthesises(c.Name), Value = c.Value }).ToList() 
                ?? new List<ConstantColumnConfig>();

            normalizedTableConfig.ScrambledColumns = tableConfig.ScrambledColumns?.Select(c =>
            new ScrambledColumnConfig { Name = ParameterNameHelper.RemoveParenthesises(c.Name) }).ToList()
                ?? new List<ScrambledColumnConfig>();

            normalizedTableConfig.PairedColumnsInsideTable = tableConfig.PairedColumnsInsideTable?
                .Select(l => ParameterNameHelper.RemoveParenthesisesFromStringList(l)).ToList()
                ?? new List<List<string>>();

            normalizedTableConfig.PairedColumnsOutsideTable = tableConfig.PairedColumnsOutsideTable?
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
                    ?? new List<PairedColumnsOutsideTableConfig>();

            normalizedTableConfig.FullTableName = ParameterNameHelper.AddParenthesisToFullTableName(tableConfig.FullTableName);

            return normalizedTableConfig;
        }
    }
}
