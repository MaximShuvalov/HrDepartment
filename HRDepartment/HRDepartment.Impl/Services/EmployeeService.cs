using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRDepartment.Core.Services;
using HRDepartment.Core.UnitOfWork;
using HRDepartment.Model;

namespace HRDepartment.Impl.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Create(Employee employee)
        {
            if (employee == null) throw new ArgumentException("Employee is null");
            var allEmployees = await GetAllEmployees();
            var existingEmployee = allEmployees.FirstOrDefault(p =>
                p.PhoneNumber.Equals(employee.PhoneNumber) && p.Fio.Equals(employee.Fio));
            if (existingEmployee == null)
            {
                using (_unitOfWork)
                {
                    await _unitOfWork.GetRepositories<Employee>().Create(employee);
                    _unitOfWork.Commit();
                }
            }
            else
            {
                if (existingEmployee.Fired)
                    employee.Fired = false;
                if (existingEmployee.Departments.Count == 2)
                    throw new Exception("Невозможно устроить сотрудника больше,чем на 2, должности");

                for (int i = 0; i <= employee.Departments.Count; i++)
                {
                    if (!existingEmployee.Departments.Contains(employee.Departments[i]) &&
                        existingEmployee.Departments.Count < 2)
                        existingEmployee.Departments.Add(employee.Departments[i]);
                }

                await Update(existingEmployee);
            }
        }

        public async Task FireEmployee(Employee employee)
        {
            if (employee == null) throw new ArgumentException("Employee is null");
            var allEmployees = await GetAllEmployees();
            var existingEmployee = allEmployees.FirstOrDefault(p =>
                p.PhoneNumber.Equals(employee.PhoneNumber) && p.Fio.Equals(employee.Fio));
            existingEmployee.Fired = true;
            await Update(existingEmployee);
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
    }
}