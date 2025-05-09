using WebApp.Application.Services.DTOs;
using WebApp.Domain.Models.ViewModels;

namespace WebApp.Application.Services.Interfaces
{
    public interface IBasketService
    {
        Task<Basket> GetBasket();
        Task<Basket> UpdateBasket(Basket basket);
        Task AddItemToBasket(int productId);
        Task Checkout(BasketDto basket);
    }
}
