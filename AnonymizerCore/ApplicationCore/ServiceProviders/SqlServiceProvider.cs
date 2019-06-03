using ApplicationCore.DatabaseServices.ColumnTypes;
using ApplicationCore.DatabaseServices.PrimaryKeys;
using ApplicationCore.Factories;
using ApplicationCore.Validators.ConfigValidators;
using ApplicationCore.Validators.ParameterValidators;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationCore.ServiceProviders
{
    public static class SqlServiceProvider
    {
        public static ServiceProvider GetSqlServiceProvider()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfigValidator, SqlConfigValidator>()
                .AddSingleton<IParameterValidator, SqlParameterValidator>()
                .AddSingleton<IColumnTypeManager, SqlColumnTypesManager>()
                .AddSingleton<IPrimaryKeyManager, SqlPrimaryKeyManager>()
                .AddSingleton<ITableInfoCollectionFactory, SqlTableInfoCollectionFactory>()
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
