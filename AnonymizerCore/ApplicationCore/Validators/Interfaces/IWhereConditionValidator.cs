using Scrambler.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.Validators.Interfaces
{
    public interface IWhereConditionValidator
    {
        bool IsWhereConditionValid(string connectionString, TableConfig tableConfig);
    }
}
