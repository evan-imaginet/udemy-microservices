using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Api.DataAccess
{
    public static class DependacyInjectionExtension
    {
        public static void AddDataAccess(this IServiceCollection services)
        {
            services.AddScoped<ICatalogSeeder, CatalogSeeder>();
            services.AddScoped<ICatalogContext, CatalogContext>();
        }
    }
}
