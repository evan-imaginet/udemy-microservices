namespace Basket.Api.Model
{
    public class BasketItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }

        // unit price is a calculated value retrieved from a service
        // we should not be storing this as we don't know when that price expires
        public decimal UnitPrice { get; set; }


        // in my opinion this should be part of the product selection
        // not a cart option as different colors would likely have different skus
        public string Color { get; set; }

        public decimal TotalPrice
        {
            get
            {
                return ((decimal)(Quantity)) * UnitPrice;
            }
        }
    }
}
