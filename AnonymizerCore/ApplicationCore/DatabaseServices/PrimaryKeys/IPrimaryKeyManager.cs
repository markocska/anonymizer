using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.DatabaseServices.PrimaryKeys
{
    public interface IPrimaryKeyManager
    {
        List<string> GetPrimaryKeys(string connectionString, string tableNameWithSchema);
    }
}
