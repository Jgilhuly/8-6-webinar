using RestaurantOps.Legacy.Models;

namespace RestaurantOps.Legacy.Data
{
    public interface IInventoryRepository
    {
        void AdjustStock(int ingredientId, decimal quantityChange, string? notes);
        IEnumerable<InventoryTx> GetByIngredient(int ingredientId);
    }
} 