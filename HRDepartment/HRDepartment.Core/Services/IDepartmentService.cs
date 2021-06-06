using System.Collections.Generic;
using System.Threading.Tasks;
using HRDepartment.Model;
using HRDepartment.Model.DataBase;

namespace HRDepartment.Core.Services
{
    public interface IDepartmentService
    {
        Task Create(Department department);
        Task<List<Department>> GetAllDepartments();
        Task Delete(Department department);
        Task Update(Department department);
        Task<Department> Get(long id);
        Task SetBoss(Employee employee, long idDepartment);
        List<EmployeeLog> GetFiredEmployees(long keyDepartment);
        List<EmployeeLog> GetEmployees(long keyDepartment);
        Task RecruitEmployee(Employee employee, Department department, string position);
        Task FireEmployee(long employeeKey, long departmentKey);
    }
}