using Scrambler.Utilities;
using Scrambler.Validators.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.MySql.Validators
{
    public class MySqlLinkedServerValidator : LinkedServerValidator
    {

        private readonly IQueryHelper _queryHelper;

        public MySqlLinkedServerValidator(IQueryHelper queryHelper)
        {
            _queryHelper = queryHelper;
        }


        //MYSql has no real linked server feature
        protected override List<string> GetLinkedServerNames(string connectionString)
        {
            return new List<string>();
        }
    }
}
