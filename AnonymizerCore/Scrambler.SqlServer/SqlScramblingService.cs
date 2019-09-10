using Scrambler.DatabaseServices.ColumnTypes;
using Scrambler.DatabaseServices.PrimaryKeys;
using Scrambler.Factories;
using Scrambler.ServiceProviders;
using Scrambler.Validators.ConfigValidators;
using Scrambler.Validators.ParameterValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.SqlServer
{
    public class SqlScramblingService : 
        ScramblingService<SqlConfigValidator, SqlParameterValidator, SqlColumnTypesManager, SqlPrimaryKeyManager, SqlTableInfoCollectionFactory>
    {
        public SqlScramblingService(): base()
        {

        }
    }
}
