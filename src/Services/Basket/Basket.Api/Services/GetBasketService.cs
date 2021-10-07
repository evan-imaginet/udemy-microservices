using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Basket.Api.Services
{
    public class GetBasketService: IRequestHandler<GetBasket, Model.Basket>
    {
        private readonly IDistributedCache cache;

        public GetBasketService(IDistributedCache cache)
        {
            this.cache = cache;
        }
        public async Task<Model.Basket> Handle(GetBasket request, CancellationToken cancellationToken)
        {
            Model.Basket basket;

            var basketJson = await cache.GetStringAsync(request.Id);

            if (basketJson == null)
            {
                basket = new Model.Basket { Id = request.Id };
            }
            else
            {
                basket = JsonConvert.DeserializeObject<Model.Basket>(basketJson);
            }

            return basket;
        }
    }
}
