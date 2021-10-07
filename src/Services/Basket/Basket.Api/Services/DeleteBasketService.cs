using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading;
using System.Threading.Tasks;

namespace Basket.Api.Services
{
    public class DeleteBasketService: IRequestHandler<DeleteBasket, bool>
    {
        private readonly IDistributedCache cache;

        public DeleteBasketService(IDistributedCache cache)
        {
            this.cache = cache;
        }
        public async Task<bool> Handle(DeleteBasket request, CancellationToken cancellationToken)
        {
            await cache.RemoveAsync(request.Id, cancellationToken);

            return true;
        }
    }
}
