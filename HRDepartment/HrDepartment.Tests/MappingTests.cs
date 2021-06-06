using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HRDepartment.Impl.Mapping;
using HRDepartment.Model;
using HRDepartment.Model.DTO;
using NUnit.Framework;

namespace HrDepartment.Tests
{
    public class MappingTests
    {
        private IMapper _mapper;
        [SetUp]
        public void SetUp()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            _mapper = mappingConfig.CreateMapper();
        }
        [Test]
        public void MappingDepartmentToDepartmentFullInfoTest()
        {
            var department = new Department()
            {
                Address = "Address",
                Name = "TestName",
                Key = 123454312,
                BossId = 3,
                Boss = new Employee()
                {
                    Fio = "Тестов Тест Тестович",
                    Key = 123453,
                    PhoneNumber = "+79992347689"
                },
                EmployeeLogs = new List<EmployeeLog>()
                {
                    new EmployeeLog()
                    {
                        Department = new Department(),
                        Employee = new Employee()
                        {
                            Fio = "Тестов Тест2 Тестович",
                            Key = 123453,
                            PhoneNumber = "+79992347689"
                        }
                    }
                }
            };

            var fullInfoDepartment = _mapper.Map<Department, DepartmentFullInfo>(department);

            Assert.Equals(fullInfoDepartment.Name, department.Name);
            Assert.Equals(fullInfoDepartment.Address, department.Address);
            Assert.Equals(fullInfoDepartment.Boss, department.Boss);
            Assert.Equals(fullInfoDepartment.Employees, department.EmployeeLogs.Select(l => new ActiveEmployee()
            {
                Fio = l.Employee.Fio,
                PhoneNumber = l.Employee.PhoneNumber
            }));
        }
    }
}