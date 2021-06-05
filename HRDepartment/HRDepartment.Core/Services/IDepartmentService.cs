using System.Collections.Generic;
using System.Threading.Tasks;
using HRDepartment.Model;
using HRDepartment.Model.Api;

namespace HRDepartment.Core.Services
{
    public interface IDepartmentService
    {
        Task Create(Department department);
        Task<List<DepartmentDto>> GetAllDepartments();
        Task Delete(Department department);
        Task Update(Department department);
        Task<DepartmentDto> Get(long id);
        Task SetBoss(Employee employee, long idDepartment);
        List<Employee> GetFiredEmployees(long keyDepartment);
        List<Employee> GetEmployees(long keyDepartment);
    }
}