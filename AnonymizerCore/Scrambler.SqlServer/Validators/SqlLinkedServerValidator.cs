using Microsoft.Extensions.Logging;
using Scrambler.Utilities;
using Scrambler.Validators.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Scrambler.SqlServer.Validators
{
    public class SqlLinkedServerValidator : LinkedServerValidator
    {
        private readonly IQueryHelper _queryHelper;

        public SqlLinkedServerValidator(ILogger<SqlLinkedServerValidator> logger, IQueryHelper queryHelper)
            : base(logger)
        {
            _queryHelper = queryHelper;
        }

        protected override DataTable GetLinkedServerNames(string connectionString)
        {
            return  _queryHelper.ExecuteQueryWithoutParams(connectionString, "select * from sys.servers;");

        }
    }
}
