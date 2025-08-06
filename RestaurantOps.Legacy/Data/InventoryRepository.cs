using Microsoft.EntityFrameworkCore;
using RestaurantOps.Legacy.Models;

namespace RestaurantOps.Legacy.Data
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly RestaurantOpsContext _context;

        public InventoryRepository(RestaurantOpsContext context)
        {
            _context = context;
        }

        public void AdjustStock(int ingredientId, decimal quantityChange, string? notes)
        {
            // Record transaction
            var transaction = new InventoryTx
            {
                IngredientId = ingredientId,
                TxDate = DateTime.UtcNow,
                QuantityChange = quantityChange,
                Notes = notes
            };
            _context.InventoryTransactions.Add(transaction);

            // Update running balance
            var ingredient = _context.Ingredients.Find(ingredientId);
            if (ingredient != null)
            {
                ingredient.QuantityOnHand += quantityChange;
            }

            _context.SaveChanges();
        }

        public IEnumerable<InventoryTx> GetByIngredient(int ingredientId)
        {
            return _context.InventoryTransactions
                .Include(tx => tx.Ingredient)
                .Where(tx => tx.IngredientId == ingredientId)
                .OrderByDescending(tx => tx.TxDate)
                .ToList();
        }
    }
} 