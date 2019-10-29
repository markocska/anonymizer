using Microsoft.Extensions.Logging;
using Scrambler.Config;
using Scrambler.Validators.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Scrambler.Validators.Abstract
{
    public abstract class LinkedServerValidator : ILinkedServerValidator
    {

        public bool AreLinkedServerParamsValid(string connectionString, TableConfig tableInfo)
        {
            if (tableInfo.PairedColumnsOutsideTable == null) { return true; }

            var linkedServerNames = tableInfo.PairedColumnsOutsideTable.SelectMany(p => p.SourceDestMapping)
                .Select(s => s.DestinationLinkedInstance).Where(s => !string.IsNullOrEmpty(s)).Distinct();

            var existingServerNames = GetLinkedServerNames(connectionString);

            var notExistingServerNames = linkedServerNames.Except(existingServerNames).ToList();

            if (notExistingServerNames.Count != 0)
            {
                foreach (var notExistingServer in notExistingServerNames)
                {
                    Log.Error($"The linked server {notExistingServer} doesn't exist for database {connectionString}." +
                        $" Couldn't depersonalize table {tableInfo.FullTableName}.");
                }
                return false;
            }

            return true;
        }

        protected abstract List<string> GetLinkedServerNames(string connectionString);


    }
}
