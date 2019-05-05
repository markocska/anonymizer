using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DatabaseServices.PrimaryKeys
{
    public interface IPrimaryKeyManager
    {
        List<string> GetPrimaryKeys(string connectionString, string tableNameWithSchema);
    }
}
