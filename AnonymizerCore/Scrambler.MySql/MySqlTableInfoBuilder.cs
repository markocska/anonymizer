using MySql.Data.MySqlClient;
using Scrambler.Config;
using Scrambler.DatabaseServices.ColumnTypes;
using Scrambler.DatabaseServices.PrimaryKeys;
using Scrambler.TableInfo.Abstract;
using Scrambler.Validators;
using Scrambler.Validators.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scrambler.MySql
{
    public class MySqlTableInfoBuilder : TableInfoBuilder
    {
        public MySqlTableInfoBuilder(DatabaseConfig dbConfig, TableConfig tableConfig, IConfigValidator configValidator, IWhereConditionValidator whereConditionValidator,
         ILinkedServerValidator linkedServerValidator, IColumnTypeManager columnTypeManager, IPrimaryKeyManager primaryKeyManager) :
         base(dbConfig, tableConfig, configValidator, whereConditionValidator, linkedServerValidator, columnTypeManager, primaryKeyManager)
        {

        }

        protected override string ParseDataSource(string connectionString)
        {
            var connectionStringBuilder = new MySqlConnectionStringBuilder(DatabaseConfig.ConnectionString);

            return ParameterNameHelper.AddQuotationMarks(connectionStringBuilder.Database);
        }

        protected override (string schemaName, string tableName) ParseSchemaAndTableName(string schemaAndTableName)
        {
            var tableAndSchemaName = schemaAndTableName.Split('.');

            return (schemaName: ParameterNameHelper.AddQuotationMarks(tableAndSchemaName[0]), tableName: ParameterNameHelper.AddQuotationMarks(tableAndSchemaName[1]));
        }


        protected override TableConfig NormalizeTableConfigParameters(TableConfig tableConfig)
        {

            var normalizedTableConfig = new TableConfig
            {
                ConstantColumns = tableConfig.ConstantColumns?.Select(c =>
                new ConstantColumnConfig { Name = ParameterNameHelper.RemoveQuotationMarks(c.Name), Value = c.Value }).ToList()
                ?? new List<ConstantColumnConfig>(),

                ScrambledColumns = tableConfig.ScrambledColumns?.Select(c =>
                new ScrambledColumnConfig { Name = ParameterNameHelper.RemoveQuotationMarks(c.Name) }).ToList()
                ?? new List<ScrambledColumnConfig>(),

                PairedColumnsInsideTable = tableConfig.PairedColumnsInsideTable?
                .Select(l => ParameterNameHelper.RemoveQuotationMarksFromStringList(l)).ToList()
                ?? new List<List<string>>(),

                PairedColumnsOutsideTable = tableConfig.PairedColumnsOutsideTable?
                .Select(p =>
                    new PairedColumnsOutsideTableConfig
                    {
                        ColumnMapping = p.ColumnMapping.Select(l => ParameterNameHelper.AddQuotationMarksToStrList(l)).ToList(),
                        SourceDestMapping = p.SourceDestMapping.Select(s =>
                            new SourceDestMappingStepConfig
                            {
                                DestinationLinkedInstance = ParameterNameHelper.AddQuotationMarks(s.DestinationLinkedInstance),
                                DestinationConnectionString = s.DestinationConnectionString,
                                DestinationFullTableName = ParameterNameHelper.AddQuotationMarks(s.DestinationFullTableName),
                                ForeignKeyMapping = s.ForeignKeyMapping.Select(l => ParameterNameHelper.AddQuotationMarksToStrList(l)).ToList()
                            }).ToList()
                    }).ToList()
                    ?? new List<PairedColumnsOutsideTableConfig>(),

                FullTableName = ParameterNameHelper.AddQuotationMarksToFullTableName(tableConfig.FullTableName),
                Where = tableConfig.Where
            };

            return normalizedTableConfig;
        }
    }
}
