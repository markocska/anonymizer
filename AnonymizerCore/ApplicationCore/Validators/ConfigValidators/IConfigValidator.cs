using ApplicationCore.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Validators.ConfigValidators
{
    public interface IConfigValidator
    {
        bool IsTableConfigValid(DatabaseConfig dbConfig, TableConfig tableConfig);
        bool IsDbConfigValid(DatabaseConfig dbConfig);
    }
}
