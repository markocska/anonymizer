using Microsoft.Extensions.Logging;
using Scrambler.Config;
using Scrambler.Extensions;
using Scrambler.Logging;
using Scrambler.Validators;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Scrambler.Validators.Abstract
{
    public abstract class ParameterValidator : IParameterValidator
    {

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
                    Log.Error($"The column {column} doesn't exist in the table {logInfo.TableNameWithSchema}. " +
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
                    Log.Error($"The column {column} is part of a unique constraint in table {logInfo.TableNameWithSchema}. " +
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
                    Log.Error($"The following column is part of a primary key: {primaryKey} in the table {logInfo.TableNameWithSchema}." +
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
                Log.Error($"There are no constant or scrambled columns in the table to scramble/set. " +
                            $"{logInfo.TableNameWithSchema}. Connection string: {logInfo.ConnectionString}", logInfo);
                throw new ArgumentNullException("Constant and scrambled column lists can not be both empty.");
            }

            if ((scrambledColumns == null || constantColumns == null))
            {
                return false;
            }

            if(scrambledColumns.Count() == 0 || constantColumns.Count() == 0)
            {
                return false;
            }

            bool isThereADuplicationConflict = false;
            var normalizedScrambledColumnsCopy = GetNormalizedColumnListCopy(scrambledColumns);
            var normalizedConstantColumnsCopy = GetNormalizedColumnListCopy(constantColumns);
            foreach (var scrambledColumn in normalizedScrambledColumnsCopy)
            {
                var scrambledColumnCopy = String.Copy(scrambledColumn);
                foreach (var constantColumn in normalizedConstantColumnsCopy)
                {
                    if (constantColumn.ToLower() == scrambledColumn.ToLower())
                    {
                        Log.Error($"The column {constantColumn} appears both at the constant and scrambled columns in table " +
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

            if (pairedColumnsInside.Count == 0)
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
                    Log.Error($"The column {column} doesn't exist in the table {logInfo.TableNameWithSchema}. " +
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

            if (tableConfig.PairedColumnsOutsideTable.Count == 0)
            {
                return true;
            }

            if (tableConfig.PairedColumnsOutsideTable.First().ColumnMapping.Count == 0)
            {
                return true;
            }


            bool doAllPairedColumnsOutsideExist = true;

            foreach (var pairedColumnsOutsideConfig in tableConfig.PairedColumnsOutsideTable)
            {
                //Checking the source table's columns
                if (!DoAllSourceTableFrnKeyMapColsExist(connectionString, tableConfig, pairedColumnsOutsideConfig))
                {
                    Log.Error($"Error while checking the paired columns outside of table {tableConfig.FullTableName}. " +
                        $"Connection string: {connectionString}.");
                    doAllPairedColumnsOutsideExist = false;
                }

                //Checking the dest table's columns
                if (!DoAllDestTableFrnKeyMapColsExist(pairedColumnsOutsideConfig))
                {
                    Log.Error($"Error while checking the paired columns outside of table {tableConfig.FullTableName}. " +
                        $"Connection string: {connectionString}.");
                    doAllPairedColumnsOutsideExist = false;
                }

                //checking table columns between source and dest
                for (int i = 0; i < (pairedColumnsOutsideConfig.SourceDestMapping.Count - 1); i++)
                {
                    var mappingTable = pairedColumnsOutsideConfig.SourceDestMapping[i];
                    var nextMappingTable = pairedColumnsOutsideConfig.SourceDestMapping[i + 1];


                    if (!CheckIfTableExists(connectionString, mappingTable.DestinationConnectionString, mappingTable.DestinationFullTableName))
                    {
                        return false;
                    }
                    

                    var schemaTable = GetTableSchema(mappingTable.DestinationConnectionString, mappingTable.DestinationFullTableName);

                    var columns = mappingTable.ForeignKeyMapping.GetSubListElementsOfIndex(1)
                        .Concat(nextMappingTable.ForeignKeyMapping.GetSubListElementsOfIndex(0));
                    if (!DoAllColumnsExist(schemaTable, new LoggingInfo
                    {
                        ConnectionString = mappingTable.DestinationConnectionString,
                        TableNameWithSchema = mappingTable.DestinationFullTableName
                    }, columns))
                    {
                        doAllPairedColumnsOutsideExist = false;
                        Log.Error($"Error while checking the paired columns outside of table: {tableConfig.FullTableName}. " +
                            $"Connection string: {connectionString}. ");
                    }

                }
            }
            return doAllPairedColumnsOutsideExist;
        }

        private bool CheckIfTableExists(string connectionString, string destinationConnectionString, string destinationFullTableName)
        {
            try
            {
                var schemaTable = GetTableSchema(destinationConnectionString, destinationFullTableName);
                return true;
            }
            catch (SqlException ex)
            {
                Log.Error($"Error while checking the paired columns outside of table: {destinationConnectionString}. " +
               $"Connection string: {connectionString}. " +
               $"The mapped database {destinationConnectionString} or table {destinationFullTableName} doesn't exist or it is unreachable. " +
               $"Error message: {ex.Message}.");
                return false;
            }
        }

        protected abstract IEnumerable<string> GetNormalizedColumnListCopy(IEnumerable<string> columns);

        private bool DoAllSourceTableFrnKeyMapColsExist(string connectionString, TableConfig tableConfig,
            PairedColumnsOutsideTableConfig pairedColumnsOutsideConfig)
        {

            var columnsInSourceTable = pairedColumnsOutsideConfig.ColumnMapping.GetSubListElementsOfIndex(0)
                   .Concat(pairedColumnsOutsideConfig.SourceDestMapping.First().ForeignKeyMapping
                       .GetSubListElementsOfIndex(0));

            var sourceSchemaTable = GetTableSchema(connectionString, tableConfig.FullTableName);

            if (!DoAllColumnsExist(sourceSchemaTable, new LoggingInfo { ConnectionString = connectionString, TableNameWithSchema = tableConfig.FullTableName },
                columnsInSourceTable))
            {
                Log.Error($"Error while checking the columns of table: {tableConfig.FullTableName}. " +
                        $"Connection string: {connectionString}. ");
                return false;
            }

            return true;
        }

        private bool DoAllDestTableFrnKeyMapColsExist(PairedColumnsOutsideTableConfig pairedColumnsOutsideConfig)
        {
            string connectionString = pairedColumnsOutsideConfig.SourceDestMapping.Last().DestinationConnectionString;
            string fullTableName = pairedColumnsOutsideConfig.SourceDestMapping.Last().DestinationFullTableName;

            var columnsInSourceTable = pairedColumnsOutsideConfig.ColumnMapping.GetSubListElementsOfIndex(1)
                   .Concat(pairedColumnsOutsideConfig.SourceDestMapping.Last().ForeignKeyMapping
                       .GetSubListElementsOfIndex(1));

            DataTable destSchemaTable;

            try
            {
                destSchemaTable = GetTableSchema(connectionString, fullTableName);
            }
            catch (SqlException ex)
            {
                Log.Error($"The mapped database {connectionString} or table {fullTableName} doesn't exist or it is unreachable. " +
                        $"Error message: {ex.Message}");
                return false;
            }

            if (!DoAllColumnsExist(destSchemaTable, new LoggingInfo { ConnectionString = connectionString, TableNameWithSchema = fullTableName },
                columnsInSourceTable))
            {
                Log.Error($"Error while checking the columns of table: {fullTableName}. " +
                        $"Connection string: {connectionString}. ");
                return false;
            }

            return true;
        }
    }
}
