using MediatR;

namespace Basket.Api.Services
{
    public class DeleteBasket: IRequest<bool>
    {
        public string Id { get; set; }
    }
}
