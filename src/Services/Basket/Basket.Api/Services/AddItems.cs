using Basket.Api.Model;
using MediatR;
using System.Collections.Generic;

namespace Basket.Api.Services
{
    public class AddItems: IRequest<Model.Basket>
    {
        public string Id { get; set; }
        public List<BasketItem> Items { get; set; }
    }
}
