using Movie_Web_Api.Models;

namespace Movie_Web_Api.Repository
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole);
        Task StoreOrderAsync(List<CartItem> items, string userId, string userEmailAddress);
    }
}