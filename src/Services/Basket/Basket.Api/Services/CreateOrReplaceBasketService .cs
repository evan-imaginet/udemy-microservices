using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Basket.Api.Services
{
    public class CreateOrReplaceBasketService: IRequestHandler<CreateOrReplaceBasket, Model.Basket>
    {
        private readonly IDistributedCache cache;

        public CreateOrReplaceBasketService(IDistributedCache cache)
        {
            this.cache = cache;
        }
        public async Task<Model.Basket> Handle(CreateOrReplaceBasket request, CancellationToken cancellationToken)
        {
            var json = JsonConvert.SerializeObject(request.Basket);
            await cache.SetStringAsync(request.Basket.Id, json, cancellationToken);

            return request.Basket;
        }
    }
}
