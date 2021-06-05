using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRDepartment.Core.Context;
using HRDepartment.Core.Repositories;
using HRDepartment.Model;
using Microsoft.EntityFrameworkCore;

namespace HRDepartment.Impl.Repositories
{
    public class EmployeeRepositories : IRepository<Employee>
    {
        private readonly HrDepartmentContext _context;

        public EmployeeRepositories(HrDepartmentContext context)
        {
            _context = context;
        }

        public async Task Create(Employee item) => await _context.Employees.AddAsync(item);

        public async Task Delete(Employee item) => await Task.Run(() => _context.Employees.Remove(item));

        public async Task<Employee> Get(long id)
        {
            return await _context.Employees.FirstOrDefaultAsync(p => p.Key.Equals(id));
        }

        public async Task Update(Employee item) => await Task.Run(() => _context.Employees.Update(item));

        public async Task<List<Employee>> GetAllAsync() => await Task.Run(() => _context.Employees.ToList());

        public List<Employee> GetAll() => _context.Employees.ToList();
    }
}