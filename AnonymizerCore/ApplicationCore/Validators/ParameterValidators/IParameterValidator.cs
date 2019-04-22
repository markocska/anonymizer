using ApplicationCore.Config;
using ApplicationCore.TableInfo;
using System.Collections.Generic;

namespace ApplicationCore.Validators.ParameterValidators
{
    public interface IParameterValidator
    {
        bool AreTheParamsValid(string connectionString, TableConfig tableConfig);
    }
}