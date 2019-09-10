using Scrambler.DatabaseServices.ColumnTypes;
using Scrambler.DatabaseServices.PrimaryKeys;
using Scrambler.Factories;
using Scrambler.Validators.ConfigValidators;
using Scrambler.Validators.ParameterValidators;
using Microsoft.Extensions.DependencyInjection;

namespace Scrambler.ServiceProviders
{
    public static class SqlServiceProvider { 
        public static ServiceProvider GetServiceProvider()
        {
            var serviceProvider = new ServiceCollection()
                .AddScoped<IConfigValidator, SqlConfigValidator>()
                .AddScoped<IParameterValidator, SqlParameterValidator>()
                .AddScoped<IColumnTypeManager, SqlColumnTypesManager>()
                .AddScoped<IPrimaryKeyManager, SqlPrimaryKeyManager>()
                .AddScoped<ITableInfoCollectionFactory, SqlTableInfoCollectionFactory>()
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
