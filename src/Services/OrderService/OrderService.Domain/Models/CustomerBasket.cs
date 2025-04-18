using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Models
{
    public class CustomerBasket
    {
        public string BuyerId { get; set; }
        public List<BasketItem> BasketItems { get; set; }

        public CustomerBasket(string customerId)
        {
            BuyerId = customerId;
            BasketItems = new List<BasketItem>();
        }
    }
}
