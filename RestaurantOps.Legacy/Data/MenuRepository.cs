using Microsoft.EntityFrameworkCore;
using RestaurantOps.Legacy.Models;

namespace RestaurantOps.Legacy.Data
{
    public class MenuRepository : IMenuRepository
    {
        private readonly RestaurantOpsContext _context;

        public MenuRepository(RestaurantOpsContext context)
        {
            _context = context;
        }

        public IEnumerable<MenuItem> GetAll()
        {
            return _context.MenuItems
                .Include(mi => mi.Category)
                .OrderBy(mi => mi.Category!.Name)
                .ThenBy(mi => mi.Name)
                .ToList();
        }

        public void Add(MenuItem item)
        {
            _context.MenuItems.Add(item);
            _context.SaveChanges();
        }
    }
} 