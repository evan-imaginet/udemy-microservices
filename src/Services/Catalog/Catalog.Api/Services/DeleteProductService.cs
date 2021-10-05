using Catalog.Api.DataAccess;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Api.Services
{
    public class DeleteProductService: IRequestHandler<DeleteProduct, bool>
    {
        private readonly ILogger<DeleteProductService> logger;
        private readonly ICatalogContext context;

        public DeleteProductService(ILogger<DeleteProductService> logger, ICatalogContext context)
        {
            this.logger = logger;
            this.context = context;
        }
        public async Task<bool> Handle(DeleteProduct request, CancellationToken cancellationToken)
        {
            var result = await context.Products
                .DeleteOneAsync(x => x.Id == request.Id);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
