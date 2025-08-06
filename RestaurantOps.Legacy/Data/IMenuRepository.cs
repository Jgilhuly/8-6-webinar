using RestaurantOps.Legacy.Models;

namespace RestaurantOps.Legacy.Data
{
    public interface IMenuRepository
    {
        IEnumerable<MenuItem> GetAll();
        void Add(MenuItem item);
    }
} 