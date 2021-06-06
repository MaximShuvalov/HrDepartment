using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HRDepartment.Core.Services;
using HRDepartment.Model;
using HRDepartment.Model.Api;
using Microsoft.AspNetCore.Mvc;

namespace HRDepartment.ApiService.Controllers
{
    [Route("api")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly IEmployeeLogService _employeeLogService;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public HomeController(IDepartmentService departmentService, IEmployeeLogService employeeLogService,
            IEmployeeService employeeService, IMapper mapper)
        {
            _departmentService = departmentService;
            _employeeLogService = employeeLogService;
            _employeeService = employeeService;
            _mapper = mapper;
        }

        [HttpGet("All")]
        public async Task<List<DepartmentDto>> Get()
        {
            var departments = await _departmentService.GetAllDepartments();
            return _mapper.Map<List<Department>, List<DepartmentDto>>(departments);
        }

        [HttpGet("Department")]
        public async Task<DepartmentFullInfo> Get(long key)
        {
            return _mapper.Map<Department, DepartmentFullInfo>(await _departmentService.Get(key));
        }

        [HttpGet("Firedemployees")]
        public async Task<List<FiredEmployee>> GetFiredEmployees(long key)
        {
            var department = await _departmentService.Get(key);
            return _mapper.Map<List<EmployeeLog>, List<FiredEmployee>>(department.EmployeeLogs
                .Where(p => p.Fired == true).ToList());
        }

        [HttpPost("Recruit")]
        public async Task Recruit(Employee employee, long departmentKey, string position)
        {
            if (string.IsNullOrWhiteSpace(position)) throw new ApplicationException("Не указана должность");
            var existingEmployee = await _employeeService.GetIfExistOrNull(employee);
            if (existingEmployee == null)
            {
                await _employeeService.Create(employee);
                var createdEmployee = await _employeeService.GetIfExistOrNull(employee);
                await _departmentService.RecruitEmployee(createdEmployee, await _departmentService.Get(departmentKey),
                    position);
            }
            else if (!await _employeeService.СheckIfIsPossibleRecruitEmployee(existingEmployee))
                await _departmentService.RecruitEmployee(employee, await _departmentService.Get(departmentKey),
                    position);
            else if (await _employeeService.СheckIfIsPossibleRecruitEmployee(existingEmployee))
                throw new Exception("Невозможно устроить сотрудника больше чем в 2 отдела");
        }

        [HttpPost("Fire")]
        public async Task Fire(long employeeKey, long departmentKey)
        {
            await _departmentService.FireEmployee(employeeKey, departmentKey);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Department department)
        {
            await _departmentService.Create(department);
            return Ok();
        }
    }
}