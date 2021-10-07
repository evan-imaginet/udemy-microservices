using Basket.Api.Model;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Basket.Api.Services
{
    public class RemoveItemsService: IRequestHandler<RemoveItems, Model.Basket>
    {
        private readonly IDistributedCache cache;
        private readonly IMediator mediator;

        public RemoveItemsService(IDistributedCache cache, IMediator mediator)
        {
            this.cache = cache;
            this.mediator = mediator;
        }

        public async Task<Model.Basket> Handle(RemoveItems request, CancellationToken cancellationToken)
        {
            var basket = await mediator.Send(new GetBasket { Id = request.Id });

            basket = UpdateBasket(basket, request.Items);

            var basketJson = JsonConvert.SerializeObject(basket);
            await cache.SetStringAsync(request.Id, basketJson);

            return basket;
        }

        private Model.Basket UpdateBasket(Model.Basket basket, List<BasketItem> removals)
        {
            foreach (var removal in removals)
            {
                var item = basket.Items.FirstOrDefault(x => x.ProductId == removal.ProductId && x.Color == removal.Color);

                if (item != null)
                {
                    item.Quantity -= removal.Quantity;

                    if (item.Quantity < 1)
                    {
                        basket.Items.Remove(item);
                    }
                }
            }

            return basket;
        }
    }
}
