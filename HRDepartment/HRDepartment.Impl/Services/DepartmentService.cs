using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRDepartment.Core.Services;
using HRDepartment.Core.UnitOfWork;
using HRDepartment.Model;

namespace HRDepartment.Impl.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeLogService _employeeLogService;
        private readonly IEmployeeService _employeeService;

        public DepartmentService(IUnitOfWork unitOfWork, IEmployeeLogService employeeLogService, IEmployeeService employeeService)
        {
            _unitOfWork = unitOfWork;
            _employeeLogService = employeeLogService;
            _employeeService = employeeService;
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

        public async Task RecruitEmployee(Employee employee, Department department, string position)
        {
            var existingEmployee = await _employeeService.GetIfExistOrNull(employee);
            if (existingEmployee == null)
            {
                await _employeeService.Create(employee);
                var createdEmployee = await _employeeService.GetIfExistOrNull(employee);
                await Recruit(employee, department, position);
            }
            else if (await _employeeService.СheckIfIsPossibleRecruitEmployee(existingEmployee))
                await Recruit(employee, department, position);
            else if (!await _employeeService.СheckIfIsPossibleRecruitEmployee(existingEmployee))
                throw new Exception("Невозможно устроить сотрудника больше чем в 2 отдела");
            
            
        }

        private async Task Recruit(Employee employee, Department department, string position)
        {
            using (_unitOfWork)
            {
                var existDepartment = await _unitOfWork.GetRepositories<Department>().Get(department.Key);
                await _employeeLogService.Create(new EmployeeLog()
                {
                    Department = existDepartment,
                    Employee = employee,
                    Position = position
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