using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRDepartment.Core.Services;
using HRDepartment.Core.UnitOfWork;
using HRDepartment.Model;

namespace HRDepartment.Impl.Services
{
    public sealed class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeLogService _employeeLogService;

        public DepartmentService(IUnitOfWork unitOfWork, IEmployeeLogService employeeLogService)
        {
            _unitOfWork = unitOfWork;
            _employeeLogService = employeeLogService;
        }

        public async Task Create(Department department)
        {
            if (department == null) throw new ArgumentException("Department is null");
            if (department.EmployeeLogs.Any())
                foreach (var item in department.EmployeeLogs)
                    await _employeeLogService.Create(item);
            try
            {
                using (_unitOfWork)
                {
                    await _unitOfWork.GetRepositories<Department>().Create(department);
                    _unitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Department>> GetAllDepartments()
        {
            using (_unitOfWork)
            {
                return await _unitOfWork.GetRepositories<Department>().GetAllAsync();
            }
        }

        public async Task Delete(Department department)
        {
            if (department == null) throw new ArgumentException("Department is null");
            using (_unitOfWork)
            {
                await _unitOfWork.GetRepositories<Department>().Delete(department);
                _unitOfWork.Commit();
            }
        }

        public async Task Update(Department department)
        {
            if (department == null) throw new ArgumentException("Department is null");
            using (_unitOfWork)
            {
                await _unitOfWork.GetRepositories<Department>().Update(department);
                _unitOfWork.Commit();
            }
        }

        public async Task<Department> Get(long id)
        {
            using (_unitOfWork)
            {
                var department = await _unitOfWork.GetRepositories<Department>().Get(id);
                return department;
            }
        }

        public async Task SetBoss(Employee employee, long idDepartment)
        {
            if (employee == null) throw new ArgumentException("Employee is null");
            using (_unitOfWork)
            {
                var department = await _unitOfWork.GetRepositories<Department>().Get(idDepartment);
                department.Boss = employee;
                await Update(department);
            }
        }

        public List<EmployeeLog> GetFiredEmployees(long keyDepartment)
        {
            Department department;
            using (_unitOfWork)
            {
                department = _unitOfWork.GetRepositories<Department>().Get(keyDepartment).Result;
            }

            return department.EmployeeLogs.Where(p => p.Fired == true).ToList();
        }

        public List<EmployeeLog> GetEmployees(long keyDepartment)
        {
            Department department;
            using (_unitOfWork)
            {
                department = _unitOfWork.GetRepositories<Department>().Get(keyDepartment).Result;
            }

            return department.EmployeeLogs.Where(p => p.Fired == false).ToList();
        }

        public async Task RecruitEmployee(Employee employee, Department department)
        {
            using (_unitOfWork)
            {
                var existDepartment = await _unitOfWork.GetRepositories<Department>().Get(department.Key);
                await _employeeLogService.Create(new EmployeeLog()
                {
                    Department = existDepartment,
                    Employee = employee
                });
            }
        }

        public async Task FireEmployee(long employeeKey, long departmentKey)
        {
            var department = await Get(departmentKey);
            var empLog = department.EmployeeLogs.FirstOrDefault(p => p.Employee.Key.Equals(employeeKey));
            empLog.Fired = true;
            empLog.DateOfDismissal = DateTime.Now;
            await Update(department);
        }
    }
}