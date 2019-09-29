﻿using Microsoft.Extensions.Logging;
using Scrambler.DatabaseServices.ColumnTypes;
using Scrambler.DatabaseServices.PrimaryKeys;
using Scrambler.DatabaseServices.Scrambling;
using Scrambler.Factories;
using Scrambler.SqlServer.Validators;
using Scrambler.Utilities;
using Scrambler.Utilities.QueryHelpers;
using Scrambler.Validators.ConfigValidators;
using Scrambler.Validators.ParameterValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.SqlServer
{
    public class SqlScramblingService : 
        ScramblingService<SqlConfigValidator, SqlParameterValidator, SqlColumnTypesManager, SqlPrimaryKeyManager, SqlTableInfoCollectionFactory, 
            SqlTableScramblingService, SqlWhereConditionValidator ,SqlHelper>
    {
        public SqlScramblingService(Action<ILoggingBuilder> logConfig) : base(new SqlHelper(), logConfig)
        {

        }
    }
}
