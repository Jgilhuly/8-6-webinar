using Microsoft.EntityFrameworkCore;
using RestaurantOps.Legacy.Models;

namespace RestaurantOps.Legacy.Data
{
    public class TableRepository : ITableRepository
    {
        private readonly RestaurantOpsContext _context;

        public TableRepository(RestaurantOpsContext context)
        {
            _context = context;
        }

        public IEnumerable<RestaurantTable> GetAll()
        {
            return _context.RestaurantTables
                .OrderBy(t => t.TableId)
                .ToList();
        }

        public void UpdateOccupied(int tableId, bool occupied)
        {
            var table = _context.RestaurantTables.Find(tableId);
            if (table != null)
            {
                table.IsOccupied = occupied;
                _context.SaveChanges();
            }
        }
    }
} 