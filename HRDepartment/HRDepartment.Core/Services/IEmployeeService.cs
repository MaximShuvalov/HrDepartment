using System.Collections.Generic;
using System.Threading.Tasks;
using HRDepartment.Model;

namespace HRDepartment.Core.Services
{
    public interface IEmployeeService
    {
        Task Create(Employee employee);
        Task<List<Employee>> GetAllEmployees();
        Task Delete(Employee employee);
        Task Update(Employee employee);
        Task<Employee> Get(long id);
        Task<Employee> GetIfExistOrNull(Employee employee);
        Task<bool> СheckIfIsPossibleRecruitEmployee(Employee employee);
    }
}