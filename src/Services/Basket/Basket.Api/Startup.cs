using Basket.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Reflection;

namespace Basket.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // ensure we can find our services
            var executingAssembly = Assembly.GetExecutingAssembly();
            services.AddMediatR(executingAssembly);

            // this registers an IDistributedCache singleton
            services.AddStackExchangeRedisCache(options =>
            {
                // I understand that the ConfigurationOptions are preferred, but I don't see examples of
                // how to use it at this point in time
                options.Configuration = Configuration.GetValue<string>("Redis:Servers");
            });
        
            services.AddPipelineBehaviors();

            // we add newtonsoft because we are using JsonPatch for partial updates
            services.AddControllers()
                .AddNewtonsoftJson();

            var versionFormat = "'v'VVV";
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = versionFormat;
            });

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = ApiVersion.Default;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket API - V1", Version = "v1" });
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    //NOTE: A breakpoint on the next lines never gets hit
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                    var versions = methodInfo.DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(version => $"{version.ToString(versionFormat)}" == docName);
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket API - V1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
