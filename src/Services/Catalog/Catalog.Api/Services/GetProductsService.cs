using Catalog.Api.DataAccess;
using Catalog.Api.Model;
using MediatR;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Api.Services
{
    public class GetProductsService: IRequestHandler<GetProducts, IEnumerable<Product>>
    {
        private readonly CatalogContext context;

        public GetProductsService(CatalogContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Product>> Handle(GetProducts request, CancellationToken cancellationToken)
        {
            IFindFluent<Product, Product> query = null;

            if(!string.IsNullOrWhiteSpace(request.Name))
            {
                query = context.Products.Find(x => x.Name == request.Name);
            }
            else if (!string.IsNullOrWhiteSpace(request.Category))
            {
                query = context.Products.Find(x => x.Category == request.Category);
            }
            else
            {
                query = context.Products.Find(x => true);
            }

            return await query.ToListAsync();
        }
    }
}
