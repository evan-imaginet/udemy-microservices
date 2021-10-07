using MediatR;

namespace Basket.Api.Services
{
    public class GetBasket: IRequest<Model.Basket>
    {
        public string Id { get; set; }
    }
}
