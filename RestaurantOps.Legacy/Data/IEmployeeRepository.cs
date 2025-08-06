using RestaurantOps.Legacy.Models;

namespace RestaurantOps.Legacy.Data
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll(bool includeInactive = false);
        Employee? GetById(int id);
        void Add(Employee emp);
        void Update(Employee emp);
    }
} 