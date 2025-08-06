using Microsoft.EntityFrameworkCore;
using RestaurantOps.Legacy.Models;

namespace RestaurantOps.Legacy.Data
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly RestaurantOpsContext _context;

        public IngredientRepository(RestaurantOpsContext context)
        {
            _context = context;
        }

        public IEnumerable<Ingredient> GetAll()
        {
            return _context.Ingredients
                .OrderBy(i => i.Name)
                .ToList();
        }

        public Ingredient? GetById(int id)
        {
            return _context.Ingredients.Find(id);
        }

        public void Add(Ingredient ing)
        {
            _context.Ingredients.Add(ing);
            _context.SaveChanges();
        }

        public void Update(Ingredient ing)
        {
            var existing = _context.Ingredients.Find(ing.IngredientId);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(ing);
                _context.SaveChanges();
            }
        }
    }
} 