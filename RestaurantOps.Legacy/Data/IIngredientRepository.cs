using RestaurantOps.Legacy.Models;

namespace RestaurantOps.Legacy.Data
{
    public interface IIngredientRepository
    {
        IEnumerable<Ingredient> GetAll();
        Ingredient? GetById(int id);
        void Add(Ingredient ing);
        void Update(Ingredient ing);
    }
} 