using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRDepartment.Core.Context;
using HRDepartment.Core.Repositories;
using HRDepartment.Model;
using Microsoft.EntityFrameworkCore;

namespace HRDepartment.Impl.Repositories
{
    public class EmployeeLogRepository : IRepository<EmployeeLog>
    {
        private readonly HrDepartmentContext _context;

        public EmployeeLogRepository(HrDepartmentContext context)
        {
            _context = context;
        }

        public async Task Create(EmployeeLog item) => await _context.EmployeeLog.AddAsync(item);

        public async Task Delete(EmployeeLog item) => await Task.Run(() => _context.EmployeeLog.Remove(item));

        public async Task<EmployeeLog> Get(long id)
        {
            return await _context.EmployeeLog.FirstOrDefaultAsync(p => p.Key.Equals(id));
        }

        public async Task Update(EmployeeLog item) => await Task.Run(() => _context.EmployeeLog.Update(item));

        public async Task<List<EmployeeLog>> GetAllAsync() => await Task.Run(() => _context.EmployeeLog.ToList());

        public List<EmployeeLog> GetAll() => _context.EmployeeLog.ToList();
    }
}