
using Microsoft.Extensions.Configuration;
using Standard.Customer.Application;
using Standard.Customer.Domain;
using Standard.Customer.Domain.Config;
using Standard.Customer.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCustomerSORServices(this IServiceCollection services, IConfiguration configuration)
        {
            StorageConfig storageConfig = new StorageConfig();
            configuration.GetSection("StorageConfig").Bind(storageConfig);
            services.AddSingleton(storageConfig);

            CosmosConfig cosmosConfig = new CosmosConfig();
            configuration.GetSection("CosmosConfig").Bind(cosmosConfig);
            services.AddSingleton(cosmosConfig);

            services.AddScoped<IMigrationService, CustomerService>();
            services.AddScoped<IRepository<CustomerEntity>, CustomerService>();
            return services;
        }
    }
}
