﻿using Scrambler.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.Validators.ConfigValidators
{
    public interface IConfigValidator
    {
        bool IsTableConfigValid(DatabaseConfig dbConfig, TableConfig tableConfig);
        bool IsDbConfigValid(DatabaseConfig dbConfig);
    }
}
