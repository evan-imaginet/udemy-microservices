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
    public class AddItemsService: IRequestHandler<AddItems, Model.Basket>
    {
        private readonly IDistributedCache cache;
        private readonly IMediator mediator;

        public AddItemsService(IDistributedCache cache, IMediator mediator)
        {
            this.cache = cache;
            this.mediator = mediator;
        }
        public async Task<Model.Basket> Handle(AddItems request, CancellationToken cancellationToken)
        {
            var basket = await mediator.Send(new GetBasket { Id = request.Id });

            basket = UpdateBasket(basket, request.Items);
            
            var basketJson = JsonConvert.SerializeObject(basket);
            await cache.SetStringAsync(request.Id, basketJson);

            return basket;
        }

        private Model.Basket UpdateBasket(Model.Basket basket, List<BasketItem> updates)
        {
            foreach(var item in basket.Items)
            {
                var matches = updates.Where(x => x.ProductId == item.ProductId && x.Color == item.Color).ToList();

                if(matches != null)
                {
                    item.Quantity += matches.Sum(x => x.Quantity);

                    foreach(var match in matches)
                    {
                        updates.Remove(match);
                    }
                }
            }

            basket.Items.AddRange(updates);

            return basket;
        }
    }
}
