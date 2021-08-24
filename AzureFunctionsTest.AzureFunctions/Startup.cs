using System;
using AzureFunctionsTest.Domain.Repositories.Abstract;
using AzureFunctionsTest.Domain.Repositories.Dapper;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AzureFunctionsTest.AzureFunctions.Startup))]
namespace AzureFunctionsTest.AzureFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var connectionString =
                Environment.GetEnvironmentVariable("ConnectionString") ?? throw new ArgumentException();

            builder.Services.AddScoped<ITodoRepository, DapperTodoRepository>(_ =>
                new DapperTodoRepository(connectionString));
        }
    }
}
