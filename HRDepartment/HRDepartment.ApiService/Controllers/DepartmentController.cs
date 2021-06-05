using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRDepartment.Core.Services;
using HRDepartment.Model;
using HRDepartment.Model.Api;
using Microsoft.AspNetCore.Mvc;

namespace HRDepartment.ApiService.Controllers
{
    [Route("api/department")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public HomeController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("all")]
        public async Task<List<string>> Get()
        {
            var departments = await _departmentService.GetAllDepartments();
            return departments.Select(o => o.Name).ToList();
        }

        [HttpGet("department")]
        public async Task<DepartmentDto> Get(long key)
        {
            return await _departmentService.Get(key);
        }

        [HttpGet("firedemployees")]
        public async Task<List<Employee>> GetFiredEmployees(long key)
        {
            var department = await _departmentService.Get(key);
            return department.FiredEmployees;
        }

        [HttpPut]
        public async Task<JsonResult> Put(Department department)
        {
            await _departmentService.Create(department);
            return new JsonResult($"Отдел {department.Name} успешно создан");
        }
    }
}