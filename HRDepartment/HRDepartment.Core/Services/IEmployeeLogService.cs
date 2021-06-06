using System.Collections.Generic;
using System.Threading.Tasks;
using HRDepartment.Model;
using HRDepartment.Model.DataBase;

namespace HRDepartment.Core.Services
{
    public interface IEmployeeLogService
    {
        Task Create(EmployeeLog employeeLog);
        Task<List<EmployeeLog>> GetAllEmployeeLogs();
        Task Delete(EmployeeLog employeeLog);
        Task Update(EmployeeLog employeeLog);
        Task<EmployeeLog> Get(long id);
    }
}