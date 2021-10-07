using MediatR;

namespace Basket.Api.Services
{
    public class CreateOrReplaceBasket: IRequest<Model.Basket>
    {
        public Model.Basket Basket { get; set; }
    }
}
