using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Api.Services
{
    public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger;

        public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellation, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                // this is done separate of processing the properties so it can't throw an exception
                logger.LogDebug($"Handling {typeof(TRequest).Name}");

                //Request
                var requestType = request.GetType();
                var requestProperties = new List<PropertyInfo>(requestType.GetProperties());

                foreach (var property in requestProperties)
                {
                    var value = property.GetValue(request, null);
                    logger.LogDebug($"{property.Name}: {value}");
                }
            }
            catch
            {
                // don't fail the request/reponse because of logging
            }

            try
            {
                var response = await next();

                try
                {
                    //Response
                    logger.LogDebug($"Handled {typeof(TResponse).Name}");
                }
                catch
                {
                    // don't fail the request/reponse because of logging
                }

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
