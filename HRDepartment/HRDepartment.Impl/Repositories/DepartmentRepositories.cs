using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRDepartment.Core.Context;
using HRDepartment.Core.Repositories;
using HRDepartment.Core.Services;
using HRDepartment.Model;
using Microsoft.EntityFrameworkCore;

namespace HRDepartment.Impl.Repositories
{
    public class DepartmentRepositories : IRepository<Department>
    {
        private readonly HrDepartmentContext _context;

        public DepartmentRepositories(HrDepartmentContext context)
        {
            _context = context;
        }

        public async Task Create(Department item)
        {
            if (!_context.Departments.Contains(item))
                await _context.Departments.AddAsync(item);
        }

        public async Task Delete(Department item) => await Task.Run(() => { _context.Departments.Remove(item); });

        public async Task<Department> Get(long id)
        {
            return await _context.Departments.Include(c => c.Boss).FirstOrDefaultAsync(p => p.Key.Equals(id));
        }

        public Task Update(Department item) => Task.Run(() => _context.Departments.Update(item));

        public async Task<List<Department>> GetAllAsync() =>
            await Task.Run(() => _context.Departments.Include(p => p.Boss).ToList());

        public List<Department> GetAll() => _context.Departments.Include(p => p.Boss).ToList();
    }
}