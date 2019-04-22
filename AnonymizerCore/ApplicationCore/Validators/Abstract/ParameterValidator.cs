using ApplicationCore.Config;
using ApplicationCore.Logging;
using ApplicationCore.TableInfo;
using ApplicationCore.Validators.ParameterValidators;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ApplicationCore.Validators.Abstract
{
    public abstract class ParameterValidator : IParameterValidator
    {
        protected ILogger _logger;

        public abstract bool AreTheParamsValid(string connectionString, TableConfig tableConfig);

        protected bool DoAllColumnsExist(DataTable schemaTable, LoggingInfo logInfo, IEnumerable<string> columns)
        {
            var allColumns = schemaTable
                .Columns;

            bool doAllColumnsExist = true;
            foreach (var column in columns)
            {
                if (allColumns[column] == null)
                {
                    _logger.Error($"The column {column} doesn't exist in the table {logInfo.TableNameWithSchema}. " +
                        $"Connection string: {logInfo.ConnectionString}.", logInfo);
                    doAllColumnsExist = false;
                }
            }

            return doAllColumnsExist;
        }

        protected bool IsThereAUniqueConstraintConflict(DataTable schemaTable, LoggingInfo logInfo, IEnumerable<string> columns)
        {
            var allColumns = schemaTable
                   .Columns;

            bool isThereAUniqueConstraintConflict = false;
            foreach (var column in columns)
            {
                if (allColumns[column] == null)
                {
                    continue;
                }

                if (allColumns[column].Unique)
                {
                    _logger.Error($"The column {column} doesn't exist in the table {logInfo.TableNameWithSchema}. " +
                        $"Connection string: {logInfo.ConnectionString}. ", logInfo);
                    isThereAUniqueConstraintConflict = true;
                }
            }

            return isThereAUniqueConstraintConflict;
        }

        protected bool IsThereAPrimaryKeyConflict(DataTable schemaTable, LoggingInfo logInfo, IEnumerable<string> columns)
        {
            var primaryKeys = schemaTable.PrimaryKey.Select(c => c.ColumnName);

            bool isThereAPrimaryKeyConflict = false;
            foreach (var primaryKey in primaryKeys)
            {
                if (columns.Contains(primaryKey))
                {
                    _logger.Error($"The following column is part of a primary key: {primaryKey} in the table {logInfo.TableNameWithSchema}." +
                        $" Connection string: {logInfo.ConnectionString}",
                        logInfo);
                    isThereAPrimaryKeyConflict = true;
                }
            }

            return isThereAPrimaryKeyConflict;
        }

        protected bool DoConstantScrambledDuplicatesExist(LoggingInfo logInfo, IEnumerable<string> scrambledColumns, IEnumerable<string> constantColumns)
        {
            bool isThereADuplicationConflict = false;
            foreach (var scrambledColumn in scrambledColumns)
            {
                var scrambledColumnCopy = String.Copy(scrambledColumn);
                foreach (var constantColumn in constantColumns)
                {
                    var constantColumnCopy = String.Copy(constantColumn);

                    if (constantColumnCopy.StartsWith('[') && !scrambledColumnCopy.StartsWith('['))
                    {
                        scrambledColumnCopy = '[' + scrambledColumnCopy + ']';
                    }
                    else if (!constantColumnCopy.StartsWith('[') && scrambledColumnCopy.StartsWith('['))
                    {
                        constantColumnCopy = '[' + constantColumnCopy + ']';
                    }

                    if (constantColumnCopy.ToLower() == scrambledColumnCopy.ToLower())
                    {
                        _logger.Error($"The column {constantColumnCopy} appears both at the constant and scrambled columns in table " +
                            $"{logInfo.TableNameWithSchema}. Connection string: {logInfo.ConnectionString}", logInfo);
                        isThereADuplicationConflict = true;
                    }
                }
            }

            return isThereADuplicationConflict;
        }

        protected bool DoAllPairedColumnsInsideExist(LoggingInfo logInfo, DataTable schemaTable,  List<List<string>> pairdColumnsInside)
        {
            var pairedColumns = pairdColumnsInside.SelectMany(pd => pd, (listOfLists, list) => list).Distinct();

            var allColumns = schemaTable
                 .Columns;

            bool doAllColumnsExist = true;
            foreach (var column in pairedColumns)
            {
                if (allColumns[column] == null)
                {
                    _logger.Error($"The column {column} doesn't exist in the table {logInfo.TableNameWithSchema}. " +
                        $"Connection string: {logInfo.ConnectionString}.", logInfo);
                    doAllColumnsExist = false;
                }
            }

            return doAllColumnsExist;

        }

        protected abstract DataTable GetTableSchema(string connectionString, string nameWithSchema);

        protected bool DoAllPairedColumnsOutsideExist(string connectionString, LoggingInfo logInfo, TableConfig tableConfig)
        {
            bool doAllPairedColumnsOutsideExist = true;

            foreach (var pairedColumnOutsideConfig in tableConfig.PairedColumnsOutsideTable)
            {
                var sourceDestColumnMapping = pairedColumnOutsideConfig.ColumnMapping;
                var columnsInSourceTable = sourceDestColumnMapping.SelectMany(list => list, (list, listElement) => list.ElementAt(0));
                var sourceSchemaTable = GetTableSchema(connectionString, tableConfig.NameWithSchema);

                if (!DoAllColumnsExist(sourceSchemaTable, logInfo, columnsInSourceTable))
                {
                    doAllPairedColumnsOutsideExist = false;
                    _logger.Error($"Error while checking the paired columns outside of table: {tableConfig.NameWithSchema}. " +
                            $"Connection string: {connectionString}. ");
                }

                int i = 0;
                foreach (var mappingTable in pairedColumnOutsideConfig.SourceDestMapping)
                {
                    var schemaTable = GetTableSchema(mappingTable.ConnectionString, mappingTable.TableNameWithSchema);
                    var columns = mappingTable.ForeignKeyMapping.SelectMany(list => list, (list, listElement) => list.ElementAt(1));

                    if (!DoAllColumnsExist(schemaTable, logInfo, columns))
                    {   
                        doAllPairedColumnsOutsideExist = false;
                        _logger.Error($"Error while checking the paired columns outside of table: {mappingTable.TableNameWithSchema}. " +
                            $"Connection string: {mappingTable.ConnectionString}. ");
                    }
                    i++;
                }
            }
            return doAllPairedColumnsOutsideExist;
        }
    }
}
