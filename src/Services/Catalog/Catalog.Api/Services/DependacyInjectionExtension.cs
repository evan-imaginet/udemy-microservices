using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Api.Services
{
    public static class DependacyInjectionExtension
    {
        public static void AddPipelineBehaviors(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
        }
    }
}
