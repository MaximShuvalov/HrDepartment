using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRDepartment.Core.Services;
using HRDepartment.Core.UnitOfWork;
using HRDepartment.Model;
using HRDepartment.Model.DataBase;

namespace HRDepartment.Impl.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeLogService _employeeLogService;

        public EmployeeService(IUnitOfWork unitOfWork, IEmployeeLogService employeeLogService)
        {
            _unitOfWork = unitOfWork;
            _employeeLogService = employeeLogService;
        }

        public async Task Create(Employee employee)
        {
            if (employee == null) throw new ArgumentException("Employee is null");
            if (employee.EmployeeLogs.Any())
                foreach (var item in employee.EmployeeLogs)
                    await _employeeLogService.Create(item);
            using (_unitOfWork)
            {
                await _unitOfWork.GetRepositories<Employee>().Create(employee);
                _unitOfWork.Commit();
            }
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            using (_unitOfWork)
                return await _unitOfWork.GetRepositories<Employee>().GetAllAsync();
        }

        public async Task Delete(Employee employee)
        {
            if (employee == null) throw new ArgumentException("Employee is null");
            using (_unitOfWork)
            {
                await _unitOfWork.GetRepositories<Employee>().Delete(employee);
                _unitOfWork.Commit();
            }
        }

        public async Task Update(Employee employee)
        {
            if (employee == null) throw new ArgumentException("Employee is null");
            using (_unitOfWork)
            {
                await _unitOfWork.GetRepositories<Employee>().Update(employee);
                _unitOfWork.Commit();
            }
        }

        public async Task<Employee> Get(long id)
        {
            using (_unitOfWork)
                return await _unitOfWork.GetRepositories<Employee>().Get(id);
        }

        public async Task<Employee> GetIfExistOrNull(Employee employee)
        {
            if (employee == null) throw new ArgumentException("Employee is null");
            var employees = await GetAllEmployees();
            return employees.FirstOrDefault(p =>
                p.Fio.Equals(employee.Fio) && p.PhoneNumber.Equals(employee.PhoneNumber));
        }

        public async Task<bool> СheckIfIsPossibleRecruitEmployee(Employee employee) => await Task.Run(() =>
        {
            return !(employee.EmployeeLogs.All(p => p.Fired == false) && employee.EmployeeLogs.Count.Equals(2));
        });
    }
}