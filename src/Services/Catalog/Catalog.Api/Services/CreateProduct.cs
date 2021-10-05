using Catalog.Api.Model;
using MediatR;

namespace Catalog.Api.Services
{
    public class CreateProduct: IRequest<Product>
    {
        public Product Product { get; set; }
    }
}
