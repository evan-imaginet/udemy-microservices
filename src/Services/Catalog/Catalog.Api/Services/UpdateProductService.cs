using Catalog.Api.DataAccess;
using Catalog.Api.Model;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Api.Services
{
    public class UpdateProductService: IRequestHandler<UpdateProduct, Product>
    {
        private readonly ILogger<UpdateProductService> logger;
        private readonly ICatalogContext context;

        public UpdateProductService(ILogger<UpdateProductService> logger, ICatalogContext context)
        {
            this.logger = logger;
            this.context = context;
        }
        public async Task<Product> Handle(UpdateProduct request, CancellationToken cancellationToken)
        {
            var product = await context.Products
                .Find(x => x.Id == request.Id)
                .FirstOrDefaultAsync();

            if(product != null)
            {
                var backup = JsonConvert.DeserializeObject<Product>(JsonConvert.SerializeObject(product));

                request.Updates.ApplyTo(product);

                var result = await context.Products
                    .ReplaceOneAsync(x => x.Id == product.Id, product);

                if(!result.IsAcknowledged || result.ModifiedCount == 0)
                {
                    logger.LogWarning($"There was an error updating product with id '{request.Id}'.");

                    // we can't return the updated product, we need to return the original
                    product = backup;
                }
            }
            else
            {
                logger.LogWarning($"Could not find product for id '{request.Id}' when attempting an update");
            }

            return product;
        }
    }
}
