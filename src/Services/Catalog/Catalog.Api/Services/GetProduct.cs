using Catalog.Api.Model;
using MediatR;

namespace Catalog.Api.Services
{
    public class GetProduct: IRequest<Product>
    {
        public string Id { get; set; }
    }
}
