using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HRDepartment.Core.Services;
using HRDepartment.Model;
using HRDepartment.Model.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HRDepartment.ApiService.Controllers
{
    [Route("api")]
    [ApiController]
    public class HrDepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public HrDepartmentController(IDepartmentService departmentService, IEmployeeService employeeService,
            IMapper mapper)
        {
            _departmentService = departmentService;
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
            var department = await _departmentService.Get(departmentKey);
            await _departmentService.RecruitEmployee(employee, department, position);
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