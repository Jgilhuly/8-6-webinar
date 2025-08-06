using RestaurantOps.Legacy.Models;

namespace RestaurantOps.Legacy.Data
{
    public interface IOrderRepository
    {
        Order Create(int tableId);
        Order? GetCurrentByTable(int tableId);
        Order? GetById(int orderId);
        void AddLine(int orderId, int menuItemId, int qty, decimal price);
        void SubmitOrder(int orderId);
        void CloseOrder(int orderId);
    }
} 