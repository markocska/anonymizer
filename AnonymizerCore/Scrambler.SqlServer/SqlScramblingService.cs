using Microsoft.Extensions.Logging;
using Scrambler.DatabaseServices.ColumnTypes;
using Scrambler.DatabaseServices.PrimaryKeys;
using Scrambler.DatabaseServices.Scrambling;
using Scrambler.Factories;
using Scrambler.ServiceProviders;
using Scrambler.Utilities;
using Scrambler.Validators.ConfigValidators;
using Scrambler.Validators.ParameterValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.SqlServer
{
    public class SqlScramblingService : 
        ScramblingService<SqlConfigValidator, SqlParameterValidator, SqlColumnTypesManager, SqlPrimaryKeyManager, SqlTableInfoCollectionFactory, 
            SqlTableScramblingService>
    {
        public SqlScramblingService(IQueryHelper queryHelper, ILogger<SqlScramblingService> logger): base(queryHelper, logger)
        {

        }
    }
}
