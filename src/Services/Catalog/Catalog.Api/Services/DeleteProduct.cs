using MediatR;

namespace Catalog.Api.Services
{
    public class DeleteProduct: IRequest<bool>
    {
        public string Id { get; set; }
    }
}
