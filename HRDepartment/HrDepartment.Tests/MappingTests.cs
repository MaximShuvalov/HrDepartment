using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HRDepartment.Impl.Mapping;
using HRDepartment.Model;
using HRDepartment.Model.DataBase;
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
            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

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

            Assert.True(fullInfoDepartment.Name.Equals(department.Name));
            Assert.True(fullInfoDepartment.Address.Equals(department.Address));
            Assert.True(fullInfoDepartment.Boss.Equals(department.Boss));
            var changedDepartmentEmployeeLog = department.EmployeeLogs.Select(l => new ActiveEmployee()
            {
                Fio = l.Employee.Fio,
                PhoneNumber = l.Employee.PhoneNumber
            }).FirstOrDefault();
            var fullInfoDepartmentEmployee = fullInfoDepartment.Employees.FirstOrDefault();

            Assert.True(fullInfoDepartmentEmployee.Fio.Equals(changedDepartmentEmployeeLog.Fio));
            Assert.True(fullInfoDepartmentEmployee.PhoneNumber.Equals(changedDepartmentEmployeeLog.PhoneNumber));
        }
        
        [Test]
        public void MappingEmployeeLogToFiredEmployeeTest()
        {
            var employeeLog = new EmployeeLog()
            {
                Department = new Department(),
                Employee = new Employee()
                {
                    Fio = "Тестов Тест2 Тестович",
                    Key = 123453,
                    PhoneNumber = "+79992347689"
                },
                Fired = true,
                DateOfDismissal = DateTime.Now
            };

            var mappedEmployeeLog = _mapper.Map<EmployeeLog, FiredEmployee>(employeeLog);
            
            Assert.True(employeeLog.Employee.Fio.Equals(mappedEmployeeLog.Fio));
            Assert.True(employeeLog.Employee.PhoneNumber.Equals(mappedEmployeeLog.PhoneNumber));
            Assert.True(employeeLog.DateOfDismissal.Equals(mappedEmployeeLog.DateOfFired));
        }
    }
}