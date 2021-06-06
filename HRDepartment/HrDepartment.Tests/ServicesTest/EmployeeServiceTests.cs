using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRDepartment.Core.Context;
using HRDepartment.Core.Services;
using HRDepartment.Impl.Services;
using HRDepartment.Impl.UnitOfWork;
using HRDepartment.Model;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;

namespace HrDepartment.Tests.ServicesTest
{
    public class EmployeeServiceTests
    {
        [Test]
        public async Task CreateEmployeeTest()
        {
            int countSavesChanges = 0;
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);

            dbContext.Setup(p => p.Employees).ReturnsDbSet(new List<Employee>());
            dbContext.Setup(p => p.SaveChanges()).Callback(() => { countSavesChanges++; });
            var uow = new UnitOfWork(dbContext.Object);
            var employeeLogService = new Mock<IEmployeeLogService>().Object;
            var employeeService = new EmployeeService(uow, employeeLogService);

            await employeeService.Create(new Employee()
            {
                Fio = "Иванов Александр Петрович"
            });

            Assert.True(countSavesChanges.Equals(1));
        }

        [Test]
        public void CreateEmployee_ThrowException_Test()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            var uow = new UnitOfWork(dbContext.Object);
            var employeeLogService = new Mock<IEmployeeLogService>().Object;
            var employeeService = new EmployeeService(uow, employeeLogService);

            var ex = Assert.ThrowsAsync<ArgumentException>(() => employeeService.Create(null));

            Assert.That(ex.Message.Equals("Employee is null"));
        }

        [Test]
        public async Task GetAllEmployeesTest()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            var employees = new List<Employee>
            {
                new Employee(),
                new Employee(),
                new Employee()
            };
            dbContext.Setup(p => p.Employees).ReturnsDbSet(employees);
            var uow = new UnitOfWork(dbContext.Object);
            var employeeLogService = new Mock<IEmployeeLogService>().Object;
            var employeeService = new EmployeeService(uow, employeeLogService);

            var result = await employeeService.GetAllEmployees();

            Assert.True(result.Count.Equals(3));
        }

        [Test]
        public async Task GetEmployeeTest()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            var employees = new List<Employee>
            {
                new Employee()
                {
                    Key = 123456
                }
            };
            dbContext.Setup(p => p.Employees).ReturnsDbSet(employees);
            var uow = new UnitOfWork(dbContext.Object);
            var employeeLogService = new Mock<IEmployeeLogService>().Object;
            var employeeService = new EmployeeService(uow, employeeLogService);

            var result = await employeeService.Get(123456);

            Assert.True(result != null);
        }

        [Test]
        public void DeleteEmployee_ThrowException_Test()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            var uow = new UnitOfWork(dbContext.Object);
            var employeeLogService = new Mock<IEmployeeLogService>().Object;
            var employeeService = new EmployeeService(uow, employeeLogService);

            var ex = Assert.ThrowsAsync<ArgumentException>(() => employeeService.Delete(null));

            Assert.That(ex.Message.Equals("Employee is null"));
        }

        [Test]
        public void UpdateEmployee_ThrowException_Test()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            var uow = new UnitOfWork(dbContext.Object);
            var employeeLogService = new Mock<IEmployeeLogService>().Object;
            var employeeService = new EmployeeService(uow, employeeLogService);

            var ex = Assert.ThrowsAsync<ArgumentException>(() => employeeService.Update(null));

            Assert.That(ex.Message.Equals("Employee is null"));
        }

        [Test]
        public async Task GetEmployee_ReturnEmployee_Test()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            var employees = new List<Employee>
            {
                new Employee()
                {
                    Key = 123456,
                    PhoneNumber = "+7908",
                    Fio = "Тестов тест тестович"
                }
            };
            dbContext.Setup(p => p.Employees).ReturnsDbSet(employees);
            var uow = new UnitOfWork(dbContext.Object);
            var employeeLogService = new Mock<IEmployeeLogService>().Object;
            var employeeService = new EmployeeService(uow, employeeLogService);

            var result = await employeeService.GetIfExistOrNull(employees.First());

            Assert.True(result.Fio.Equals("Тестов тест тестович"));
            Assert.True(result.PhoneNumber.Equals("+7908"));
            Assert.True(result.Key.Equals(123456));
        }

        [Test]
        public async Task GetEmployee_ReturnNull_Test()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            var employees = new List<Employee>
            {
                new Employee()
                {
                    Key = 123456,
                    PhoneNumber = "+7908",
                    Fio = "Тестов тест тестович"
                }
            };
            dbContext.Setup(p => p.Employees).ReturnsDbSet(employees);
            var uow = new UnitOfWork(dbContext.Object);
            var employeeLogService = new Mock<IEmployeeLogService>().Object;
            var employeeService = new EmployeeService(uow, employeeLogService);

            var result = await employeeService.GetIfExistOrNull(new Employee()
            {
                Fio = "Плохой Тест Плохович",
                PhoneNumber = "+7908"
            });

            Assert.True(result == null);
        }
        
        [Test]
        public void GetEmployee_ThrowException_Test()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            var uow = new UnitOfWork(dbContext.Object);
            var employeeLogService = new Mock<IEmployeeLogService>().Object;
            var employeeService = new EmployeeService(uow, employeeLogService);

            var ex = Assert.ThrowsAsync<ArgumentException>(() => employeeService.GetIfExistOrNull(null));

            Assert.That(ex.Message.Equals("Employee is null"));
        }
        
        [Test]
        public async Task СheckIfIsPossibleRecruitEmployee_ReturnTrue_Test()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            var employees = new List<Employee>
            {
                new Employee()
                {
                    Key = 123456,
                    PhoneNumber = "+7908",
                    Fio = "Тестов тест тестович",
                    EmployeeLogs = new List<EmployeeLog>()
                    {
                        new EmployeeLog()
                        {
                            Fired = false
                        },
                        new EmployeeLog()
                        {
                            Fired = false
                        }
                    }
                }
            };
            dbContext.Setup(p => p.Employees).ReturnsDbSet(employees);
            var uow = new UnitOfWork(dbContext.Object);
            var employeeLogService = new Mock<IEmployeeLogService>().Object;
            var employeeService = new EmployeeService(uow, employeeLogService);

            var result = await employeeService.СheckIfIsPossibleRecruitEmployee(employees.First());

            Assert.True(result.Equals(true));
        }
        
        [Test]
        public async Task СheckIfIsPossibleRecruitEmployee_ReturnFalse_Test()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            var employees = new List<Employee>
            {
                new Employee()
                {
                    Key = 123456,
                    PhoneNumber = "+7908",
                    Fio = "Тестов тест тестович",
                    EmployeeLogs = new List<EmployeeLog>()
                    {
                        new EmployeeLog()
                        {
                            Fired = false
                        },
                        new EmployeeLog()
                        {
                            Fired = true
                        }
                    }
                }
            };
            dbContext.Setup(p => p.Employees).ReturnsDbSet(employees);
            var uow = new UnitOfWork(dbContext.Object);
            var employeeLogService = new Mock<IEmployeeLogService>().Object;
            var employeeService = new EmployeeService(uow, employeeLogService);

            var result = await employeeService.СheckIfIsPossibleRecruitEmployee(employees.First());

            Assert.True(result.Equals(false));
        }
    }
}