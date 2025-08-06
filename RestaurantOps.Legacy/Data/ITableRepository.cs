using RestaurantOps.Legacy.Models;

namespace RestaurantOps.Legacy.Data
{
    public interface ITableRepository
    {
        IEnumerable<RestaurantTable> GetAll();
        void UpdateOccupied(int tableId, bool isOccupied);
    }
} 