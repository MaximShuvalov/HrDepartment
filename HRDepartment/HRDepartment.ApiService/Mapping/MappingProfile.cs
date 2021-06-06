using System.Linq;
using AutoMapper;
using HRDepartment.Model;
using HRDepartment.Model.DTO;

namespace HRDepartment.Impl.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmployeeLog, FiredEmployee>()
                .ForMember(x => x.Fio,
                    x => x.MapFrom(m => m.Employee.Fio))
                .ForMember(x => x.PhoneNumber,
                    x => x.MapFrom(m => m.Employee.PhoneNumber))
                .ForMember(x => x.DateOfFired,
                    x => x.MapFrom(m => m.DateOfDismissal))
                .ForMember(x => x.Position,
                    x => x.MapFrom(m => m.Position));
            CreateMap<Department, DepartmentFullInfo>()
                .ForMember(x => x.Name,
                    x => x.MapFrom(c => c.Name))
                .ForMember(x => x.Address,
                    x => x.MapFrom(m => m.Address))
                .ForMember(x => x.Employees,
                    x => x.MapFrom(m => m.EmployeeLogs.Select(l => new ActiveEmployee()
                    {
                        Fio = l.Employee.Fio,
                        PhoneNumber = l.Employee.PhoneNumber,
                        Position = l.Position
                    })))
                .ForMember(x => x.Boss,
                    x => x.MapFrom(m => m.Boss));
        }
    }
}