using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HRDepartment.Core.Services;
using HRDepartment.Model;
using HRDepartment.Model.Api;

namespace HRDepartment.Impl.Mapping
{
    public class MappingProfile : Profile
    {
        private IDepartmentService _departmentService;

        public MappingProfile(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
            CreateMap<Department, DepartmentDto>()
                .ForMember(x => x.Name,
                    x => x.MapFrom(m => m.Name))
                .ForMember(x => x.Address,
                    x => x.MapFrom(m => m.Address))
                .ForMember(x => x.Employees,
                    x => x.MapFrom(m => GetEmployee(m)))
                .ForMember(x => x.FiredEmployees,
                    x => x.MapFrom(m => GetFiredEmployees(m)))
                .ForAllOtherMembers(x => x.Ignore());
        }

        private List<EmployeeLog> GetFiredEmployees(Department department) =>
            _departmentService.GetFiredEmployees(department.Key);


        private List<Employee> GetEmployee(Department department)
        {
            var employees = _departmentService.GetEmployees(department.Key);
            var result = employees.Select(o=> o.Employee).ToList();
            result.Add(department.Boss);
            return result;
        }
    }
}