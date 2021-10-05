using Catalog.Api.DataAccess;
using Catalog.Api.Model;
using MediatR;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Api.Services
{
    public class GetProductService: IRequestHandler<GetProduct, Product>
    {
        private readonly ICatalogContext context;

        public GetProductService(ICatalogContext context)
        {
            this.context = context;
        }
        public async Task<Product> Handle(GetProduct request, CancellationToken cancellationToken)
        {
            return await context.Products
                .Find(x => x.Id == request.Id)
                .FirstOrDefaultAsync();
        }
    }
}
