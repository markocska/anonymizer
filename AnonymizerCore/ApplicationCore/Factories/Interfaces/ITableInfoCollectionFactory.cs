using System.Collections.Generic;
using Scrambler.Config;
using Scrambler.TableInfo.Interfaces;

namespace Scrambler.Factories
{
    public interface ITableInfoCollectionFactory
    {
        List<ITableInfo> CreateTableListFromConfig(DatabasesConfig databasesConfig);
    }
}