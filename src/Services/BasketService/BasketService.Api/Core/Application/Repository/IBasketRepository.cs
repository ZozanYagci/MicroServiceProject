using BasketService.Api.Core.Domain.Models;

namespace BasketService.Api.Core.Application.Repository
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string customerId); //sepet
        IEnumerable<string> GetUsers(); //Redis'in içinde bulunan bütün kullanıcıları getirecek
        Task<CustomerBasket>UpdateBasketAsync(CustomerBasket customerBasket);
        Task<bool> DeleteBasketAsync(string id);
    }
}
