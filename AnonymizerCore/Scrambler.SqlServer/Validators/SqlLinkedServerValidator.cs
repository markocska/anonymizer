using Microsoft.Extensions.Logging;
using Scrambler.Utilities;
using Scrambler.Validators.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Scrambler.SqlServer.Validators
{
    public class SqlLinkedServerValidator : LinkedServerValidator
    {
        private readonly IQueryHelper _queryHelper;

        public SqlLinkedServerValidator(IQueryHelper queryHelper)
        {
            _queryHelper = queryHelper;
        }

        protected override List<string> GetLinkedServerNames(string connectionString)
        {
            return  _queryHelper.ExecuteQueryWithoutParams(connectionString, "select * from sys.servers;").AsEnumerable().Select(r => r.Field<string>(1))
                .Select(x => "[" + x + "]" ).ToList(); ;

        }
    }
}
