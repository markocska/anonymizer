using System.Collections.Generic;
using ApplicationCore.Config;
using ApplicationCore.TableInfo.Interfaces;

namespace ApplicationCore.Factories
{
    public interface ITableInfoCollectionFactory
    {
        List<ITableInfo> CreateTableListFromConfig(DatabasesConfig databasesConfig);
    }
}