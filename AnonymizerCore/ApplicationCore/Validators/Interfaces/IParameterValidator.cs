using Scrambler.Config;
using Scrambler.TableInfo;
using System.Collections.Generic;

namespace Scrambler.Validators
{
    public interface IParameterValidator
    {
        bool AreTheParamsValid(string connectionString, TableConfig tableConfig);
    }
}