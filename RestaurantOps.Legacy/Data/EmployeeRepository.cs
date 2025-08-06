using Microsoft.EntityFrameworkCore;
using RestaurantOps.Legacy.Models;

namespace RestaurantOps.Legacy.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly RestaurantOpsContext _context;

        public EmployeeRepository(RestaurantOpsContext context)
        {
            _context = context;
        }

        public IEnumerable<Employee> GetAll(bool includeInactive = false)
        {
            var query = _context.Employees.AsQueryable();
            if (!includeInactive)
            {
                query = query.Where(e => e.IsActive);
            }
            return query.OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToList();
        }

        public Employee? GetById(int id)
        {
            return _context.Employees.Find(id);
        }

        public void Add(Employee emp)
        {
            _context.Employees.Add(emp);
            _context.SaveChanges();
        }

        public void Update(Employee emp)
        {
            var existing = _context.Employees.Find(emp.EmployeeId);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(emp);
                _context.SaveChanges();
            }
        }
    }
} 