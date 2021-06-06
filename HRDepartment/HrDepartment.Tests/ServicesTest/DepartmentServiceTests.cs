using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRDepartment.Core.Context;
using HRDepartment.Core.Services;
using HRDepartment.Core.UnitOfWork;
using HRDepartment.Impl.Services;
using HRDepartment.Impl.UnitOfWork;
using HRDepartment.Model;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;

namespace HrDepartment.Tests.ServicesTest
{
    public class DepartmentServiceTests
    {
        [Test]
        public void СheckIfIsPossibleRecruitEmployee_ThrowException_Test()
        {
            var employee = new Employee()
            {
                Fio = "Тестов Тест2 Тестович",
                Key = 123453,
                PhoneNumber = "+79992347689",
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
            };

            var uowMock = new Mock<IUnitOfWork>();
            var employeeLogServiceMock = new Mock<IEmployeeLogService>();
            var employeeServiceMock = new Mock<IEmployeeService>();
            var departmentService = new Mock<DepartmentService>(uowMock.Object, employeeLogServiceMock.Object,
                employeeServiceMock.Object);

            employeeServiceMock.Setup(p => p.GetIfExistOrNull(employee)).Returns(Task.FromResult(employee));
            var ex = Assert.ThrowsAsync<Exception>(() =>
                departmentService.Object.RecruitEmployee(employee, new Department(), "Test"));
            Assert.That(ex.Message.Equals("Невозможно устроить сотрудника больше чем в 2 отдела"));
        }

        [Test]
        public async Task FireEmployeeTest()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            var departments = new List<Department>
            {
                new Department()
                {
                    Key = 12354,
                    Name = "Test address1"
                },
                new Department()
                {
                    Key = 321545,
                    Name = "Test address2"
                },
                new Department()
                {
                    Key = 6676,
                    EmployeeLogs = new List<EmployeeLog>()
                    {
                        new EmployeeLog()
                        {
                            Employee = new Employee()
                            {
                                Key = 1234
                            },
                            Fired = false,
                            DateOfDismissal = default
                        }
                    }
                }
            };
            dbContext.Setup(p => p.Departments).ReturnsDbSet(departments);
            var uow = new UnitOfWork(dbContext.Object);
            var employeeService = new Mock<IEmployeeService>().Object;
            var employeeLogService = new Mock<IEmployeeLogService>().Object;
            var departmentService = new DepartmentService(uow, employeeLogService, employeeService);

            await departmentService.FireEmployee(1234, 6676);

            var assertDepartment = dbContext.Object.Departments.FirstOrDefault(x => x.Key.Equals(6676));

            Assert.True(assertDepartment?.EmployeeLogs.FirstOrDefault()?.Fired.Equals(true));
        }

        [Test]
        public async Task GetAllDepartmentsTest()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            var departments = new List<Department>
            {
                new Department()
                {
                    Key = 12354,
                    Name = "Test address1"
                },
                new Department()
                {
                    Key = 321545,
                    Name = "Test address2"
                },
                new Department()
                {
                    Key = 6676,
                }
            };
            dbContext.Setup(p => p.Departments).ReturnsDbSet(departments);
            var uow = new UnitOfWork(dbContext.Object);
            var employeeService = new Mock<IEmployeeService>().Object;
            var employeeLogService = new Mock<IEmployeeLogService>().Object;
            var departmentService = new DepartmentService(uow, employeeLogService, employeeService);

            var result = await departmentService.GetAllDepartments();

            Assert.True(result.Count.Equals(3));
        }

        [Test]
        public async Task GetDepartmentTest()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            var departments = new List<Department>
            {
                new Department()
                {
                    Key = 12354,
                    Name = "Test address1"
                },
                new Department()
                {
                    Key = 321545,
                    Name = "Test address2"
                },
                new Department()
                {
                    Key = 6676,
                }
            };
            dbContext.Setup(p => p.Departments).ReturnsDbSet(departments);
            var uow = new UnitOfWork(dbContext.Object);
            var employeeService = new Mock<IEmployeeService>().Object;
            var employeeLogService = new Mock<IEmployeeLogService>().Object;
            var departmentService = new DepartmentService(uow, employeeLogService, employeeService);

            var result = await departmentService.Get(12354);

            Assert.True(result.Name.Equals("Test address1"));
        }
        
        [Test]
        public void GetEmployeeTest()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            var departments = new List<Department>
            {
                new Department()
                {
                    Key = 12354,
                    Name = "Test address1",
                    EmployeeLogs = new List<EmployeeLog>()
                    {
                        new EmployeeLog()
                        {
                            Fired = false
                        }
                    }
                }
            };
            dbContext.Setup(p => p.Departments).ReturnsDbSet(departments);
            var uow = new UnitOfWork(dbContext.Object);
            var employeeService = new Mock<IEmployeeService>().Object;
            var employeeLogService = new Mock<IEmployeeLogService>().Object;
            var departmentService = new DepartmentService(uow, employeeLogService, employeeService);

            var result = departmentService.GetEmployees(12354);

            Assert.True(result.Count.Equals(1));
        }
        
        [Test]
        public void GetFiredTest()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            var departments = new List<Department>
            {
                new Department()
                {
                    Key = 12354,
                    Name = "Test address1",
                    EmployeeLogs = new List<EmployeeLog>()
                    {
                        new EmployeeLog()
                        {
                            Fired = true
                        }
                    }
                }
            };
            dbContext.Setup(p => p.Departments).ReturnsDbSet(departments);
            var uow = new UnitOfWork(dbContext.Object);
            var employeeService = new Mock<IEmployeeService>().Object;
            var employeeLogService = new Mock<IEmployeeLogService>().Object;
            var departmentService = new DepartmentService(uow, employeeLogService, employeeService);

            var result = departmentService.GetFiredEmployees(12354);

            Assert.True(result.Count.Equals(1));
        }

        [Test]
        public void UpdateDepartment_ThrowException_Test()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            var uow = new UnitOfWork(dbContext.Object);
            var employeeService = new Mock<IEmployeeService>().Object;
            var employeeLogService = new Mock<IEmployeeLogService>().Object;
            var departmentService = new DepartmentService(uow, employeeLogService, employeeService);

            var ex = Assert.ThrowsAsync<ArgumentException>(() => departmentService.Update(null));

            Assert.That(ex.Message.Equals("Department is null"));
        }

        [Test]
        public async Task CreateDepartmentTest()
        {
            int countSavesChanges = 0;
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);

            dbContext.Setup(p => p.Departments).ReturnsDbSet(new List<Department>());
            dbContext.Setup(p => p.SaveChanges()).Callback(() => { countSavesChanges++; });
            var uow = new UnitOfWork(dbContext.Object);
            var employeeService = new Mock<IEmployeeService>().Object;
            var employeeLogService = new Mock<IEmployeeLogService>().Object;
            var departmentService = new DepartmentService(uow, employeeLogService, employeeService);

            await departmentService.Create(new Department()
            {
                Key = 6678
            });

            Assert.True(countSavesChanges.Equals(1));
        }

        [Test]
        public void CreateDepartment_ThrowException_Test()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            var uow = new UnitOfWork(dbContext.Object);
            var employeeService = new Mock<IEmployeeService>().Object;
            var employeeLogService = new Mock<IEmployeeLogService>().Object;
            var departmentService = new DepartmentService(uow, employeeLogService, employeeService);

            var ex = Assert.ThrowsAsync<ArgumentException>(() => departmentService.Create(null));
            Assert.That(ex.Message.Equals("Department is null"));
        }

        [Test]
        public async Task DeleteDepartmentTest()
        {
            int countSavesChanges = 0;
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);

            dbContext.Setup(p => p.Departments).ReturnsDbSet(new List<Department>());
            dbContext.Setup(p => p.SaveChanges()).Callback(() => { countSavesChanges++; });
            var departments = new List<Department>
            {
                new Department()
                {
                    Key = 12354,
                    Name = "Test address1"
                },
                new Department()
                {
                    Key = 321545,
                    Name = "Test address2"
                },
                new Department()
                {
                    Key = 6676,
                    EmployeeLogs = new List<EmployeeLog>()
                    {
                        new EmployeeLog()
                        {
                            Employee = new Employee()
                            {
                                Key = 1234
                            },
                            Fired = false,
                            DateOfDismissal = default
                        }
                    }
                }
            };
            var uow = new UnitOfWork(dbContext.Object);
            var employeeService = new Mock<IEmployeeService>().Object;
            var employeeLogService = new Mock<IEmployeeLogService>().Object;
            var departmentService = new DepartmentService(uow, employeeLogService, employeeService);

            await departmentService.Delete(departments.First());

            Assert.True(countSavesChanges.Equals(1));
        }

        [Test]
        public void DeleteDepartment_ThrowException_Test()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            var uow = new UnitOfWork(dbContext.Object);
            var employeeService = new Mock<IEmployeeService>().Object;
            var employeeLogService = new Mock<IEmployeeLogService>().Object;
            var departmentService = new DepartmentService(uow, employeeLogService, employeeService);

            var ex = Assert.ThrowsAsync<ArgumentException>(() => departmentService.Delete(null));
            Assert.That(ex.Message.Equals("Department is null"));
        }
        
        [Test]
        public void SetBoss_ThrowException_Test()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            var uow = new UnitOfWork(dbContext.Object);
            var employeeService = new Mock<IEmployeeService>().Object;
            var employeeLogService = new Mock<IEmployeeLogService>().Object;
            var departmentService = new DepartmentService(uow, employeeLogService, employeeService);

            var ex = Assert.ThrowsAsync<ArgumentException>(() => departmentService.SetBoss(null, 12134));
            Assert.That(ex.Message.Equals("Employee is null"));
        }
    }
}