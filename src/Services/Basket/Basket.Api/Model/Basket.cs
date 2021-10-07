using System.Collections.Generic;
using System.Linq;

namespace Basket.Api.Model
{
    public class Basket
    {
        public string Id { get; set; }
        public List<BasketItem> Items { get; private set; } = new List<BasketItem>();

        public decimal TotalPrice
        {
            get
            {
                return Items.Sum(x => x.TotalPrice);
            }
        }
    }
}
