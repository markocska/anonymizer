using Scrambler.MySql.DatabaseServices;
using Scrambler.MySql.Utilities;
using Scrambler.MySql.Validators;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.MySql
{
    public class MySqlScramblingService :
         ScramblingService<MySqlConfigValidator, MySqlParameterValidator, MySqlColumnTypesManager, MySqlPrimaryKeyManager, MySqlTableInfoCollectionFactory,
            MySqlTableScramblingService, MySqlWhereConditionValidator, MySqlLinkedServerValidator, MySqlHelper>
    {
        public MySqlScramblingService(LoggerConfiguration logConfig) : base(new MySqlHelper(), logConfig)
        {

        }
    }
}
