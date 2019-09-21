using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Data;

namespace Scrambler.Utilities
{
    public interface IQueryHelper
    {
        void ExecuteNonQueryWithoutParams(string connectionString, string cmdStr);

        DataTable ExecuteQueryWithoutParams(string connectionString, string cmdStr);

    }
}
