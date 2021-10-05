using Catalog.Api.DataAccess;
using Catalog.Api.Model;
using MediatR;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Api.Services
{
    public class CreateProductService: IRequestHandler<CreateProduct, Product>
    {
        private readonly ICatalogContext context;

        public CreateProductService(ICatalogContext context)
        {
            this.context = context;
        }
        public async Task<Product> Handle(CreateProduct request, CancellationToken cancellationToken)
        {
            await context.Products.InsertOneAsync(request.Product);

            // this should have the id that MongoDB sets on the object assigned to the object now
            return request.Product;
        }
    }
}
