using WebApp.Application.Services.DTOs;
using WebApp.Application.Services.Interfaces;
using WebApp.Domain.Models.ViewModels;

namespace WebApp.Application.Services
{
    public class OrderService : IOrderService
    {
        public BasketDto MapOrderToBasket(Order order)
        {
            order.CardExpirationApiFormat();

            return new BasketDto()
            {
                City = order.City,
                Street = order.Street,
                State = order.State,
                Country = order.Country,
                ZipCode = order.ZipCode,
                CardNumber = order.CardNumber,
                CardHolderName = order.CardHolderName,
                CardExpiration = order.CardExpiration,
                CardSecurityNumber = order.CardSecurityNumber,
                CardTypeId = 1,
                Buyer = order.Buyer
            };
        }
    }
}
