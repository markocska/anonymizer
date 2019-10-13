using Scrambler.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.Validators.Interfaces
{
    public interface ILinkedServerValidator
    {
        bool AreLinkedServerParamsValid(string connection, TableConfig tableInfo);
    }
}
