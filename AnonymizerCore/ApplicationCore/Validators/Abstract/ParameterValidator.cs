﻿using ApplicationCore.Config;
using ApplicationCore.Extensions;
using ApplicationCore.Logging;
using ApplicationCore.Validators.ParameterValidators;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ApplicationCore.Validators.Abstract
{
    public abstract class ParameterValidator : IParameterValidator
    {
        protected ILogger _logger;

        public abstract bool AreTheParamsValid(string connectionString, TableConfig tableConfig);

        protected abstract DataTable GetTableSchema(string connectionString, string nameWithSchema);

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
                    _logger.Error($"The column {column} is part of a unique constraint in table {logInfo.TableNameWithSchema}. " +
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
            if (scrambledColumns == null && constantColumns == null)
            {
                _logger.Error($"There are no constant or scrambled columns in the table to scramble/set. " +
                            $"{logInfo.TableNameWithSchema}. Connection string: {logInfo.ConnectionString}", logInfo);
                throw new ArgumentNullException("Constant and scrambled column lists can not be both empty.");
            }

            if (scrambledColumns == null || constantColumns == null)
            {
                return false;
            }

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

        protected bool DoAllPairedColumnsInsideExist(LoggingInfo logInfo, DataTable schemaTable, List<List<string>> pairedColumnsInside)
        {
            if (pairedColumnsInside == null)
            {
                return true;
            }

            var pairedColumns = pairedColumnsInside.SelectMany(pd => pd, (listOfLists, list) => list).Distinct();

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

        protected bool DoAllPairedColumnsOutsideExist(string connectionString, TableConfig tableConfig)
        {
            if (tableConfig.PairedColumnsOutsideTable == null)
            {
                return true;
            }

            bool doAllPairedColumnsOutsideExist = true;

            foreach (var pairedColumnsOutsideConfig in tableConfig.PairedColumnsOutsideTable)
            {
                //Checking the source table's columns
                if (!DoAllSourceTableFrnKeyMapColsExist(connectionString, tableConfig, pairedColumnsOutsideConfig))
                {
                    _logger.Error($"Error while checking the paired columns outside of table {tableConfig.NameWithSchema}. " +
                        $"Connection string: {connectionString}.");
                    doAllPairedColumnsOutsideExist = false;
                }

                //Checking the dest table's columns
                if (!DoAllDestTableFrnKeyMapColsExist(pairedColumnsOutsideConfig))
                {
                    _logger.Error($"Error while checking the paired columns outside of table {tableConfig.NameWithSchema}. " +
                        $"Connection string: {connectionString}.");
                    doAllPairedColumnsOutsideExist = false;
                }

                for (int i = 0; i < (pairedColumnsOutsideConfig.SourceDestMapping.Count - 1); i++)
                {
                    var mappingTable = pairedColumnsOutsideConfig.SourceDestMapping[i];
                    var nextMappingTable = pairedColumnsOutsideConfig.SourceDestMapping[i + 1];

                    DataTable schemaTable;
                    try
                    {
                        schemaTable = GetTableSchema(mappingTable.DestinationConnectionString, mappingTable.DestinationTableNameWithSchema);
                    }
                    catch (SqlException ex)
                    {
                        _logger.Error($"Error while checking the paired columns outside of table: {tableConfig.NameWithSchema}. " +
                       $"Connection string: {connectionString}. " +
                       $"The mapped database {mappingTable.DestinationConnectionString} or table {mappingTable.DestinationTableNameWithSchema} doesn't exist or it is unreachable. " +
                       $"Error message: {ex.Message}.");
                        doAllPairedColumnsOutsideExist = false;
                        continue;
                    }

                    var columns = mappingTable.ForeignKeyMapping.GetSubListElementsOfIndex(1)
                        .Concat(nextMappingTable.ForeignKeyMapping.GetSubListElementsOfIndex(0));
                    if (!DoAllColumnsExist(schemaTable, new LoggingInfo {ConnectionString = mappingTable.DestinationConnectionString,
                        TableNameWithSchema = mappingTable.DestinationTableNameWithSchema }, columns))
                    {
                        doAllPairedColumnsOutsideExist = false;
                        _logger.Error($"Error while checking the paired columns outside of table: {tableConfig.NameWithSchema}. " +
                            $"Connection string: {connectionString}. ");
                    }

                }
            }
            return doAllPairedColumnsOutsideExist;
        }

        private bool DoAllSourceTableFrnKeyMapColsExist(string connectionString, TableConfig tableConfig,
            PairedColumnsOutsideTableConfig pairedColumnsOutsideConfig)
        {
            var columnsInSourceTable = pairedColumnsOutsideConfig.ColumnMapping.GetSubListElementsOfIndex(0)
                   .Concat(pairedColumnsOutsideConfig.SourceDestMapping.First().ForeignKeyMapping
                       .GetSubListElementsOfIndex(0));

            var sourceSchemaTable = GetTableSchema(connectionString, tableConfig.NameWithSchema);

            if (!DoAllColumnsExist(sourceSchemaTable, new LoggingInfo {ConnectionString = connectionString, TableNameWithSchema = tableConfig.NameWithSchema },
                columnsInSourceTable))
            {
                _logger.Error($"Error while checking the columns of table: {tableConfig.NameWithSchema}. " +
                        $"Connection string: {connectionString}. ");
                return false;
            }

            return true;
        }

        private bool DoAllDestTableFrnKeyMapColsExist(PairedColumnsOutsideTableConfig pairedColumnsOutsideConfig)
        {
            string connectionString = pairedColumnsOutsideConfig.SourceDestMapping.Last().DestinationConnectionString;
            string tableNameWithSchema = pairedColumnsOutsideConfig.SourceDestMapping.Last().DestinationTableNameWithSchema;

            var columnsInSourceTable = pairedColumnsOutsideConfig.ColumnMapping.GetSubListElementsOfIndex(1)
                   .Concat(pairedColumnsOutsideConfig.SourceDestMapping.Last().ForeignKeyMapping
                       .GetSubListElementsOfIndex(1));

            DataTable destSchemaTable;

            try
            {
                destSchemaTable = GetTableSchema(connectionString, tableNameWithSchema);
            }
            catch (SqlException ex)
            {
                _logger.Error($"The mapped database {connectionString} or table {tableNameWithSchema} doesn't exist or it is unreachable. " +
                        $"Error message: {ex.Message}");
                return false;
            }

            if (!DoAllColumnsExist(destSchemaTable, new LoggingInfo {ConnectionString = connectionString, TableNameWithSchema = tableNameWithSchema },
                columnsInSourceTable))
            {
                _logger.Error($"Error while checking the columns of table: {tableNameWithSchema}. " +
                        $"Connection string: {connectionString}. ");
                return false;
            }

            return true;
        }
    }
}
