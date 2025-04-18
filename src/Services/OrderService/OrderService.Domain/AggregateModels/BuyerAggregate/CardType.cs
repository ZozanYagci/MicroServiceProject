using OrderService.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.AggregateModels.BuyerAggregate
{
    public class CardType : Enumeration
    {
        public static CardType Anex = new(1, nameof(Anex));
        public static CardType Visa = new(2, nameof(Visa));
        public static CardType MasterCard = new(3, nameof(MasterCard));

        public CardType(int id, string name) : base(id, name)
        {

        }
    }
}
