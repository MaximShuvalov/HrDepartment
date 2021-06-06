using System;
using System.Collections.Generic;
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

namespace HrDepartment.Tests
{
    public class EmployeeLogServiceTests
    {
        [Test]
        public async Task CreateEmployeeLogTest()
        {
            int countSavesChanges = 0;
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            dbContext.Setup(p => p.EmployeeLog).ReturnsDbSet(new List<EmployeeLog>());
            dbContext.Setup(p => p.SaveChanges()).Callback(() => { countSavesChanges++; });
            var uow = new UnitOfWork(dbContext.Object);
            var employeeLogService = new EmployeeLogService(uow);

            await employeeLogService.Create(new EmployeeLog()
            {
               Position = "C# Developer"
            });

            Assert.True(countSavesChanges.Equals(1));
        }

        [Test]
        public void CreateEmployeeLog_ThrowException_Test()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            dbContext.Setup(p => p.EmployeeLog).ReturnsDbSet(new List<EmployeeLog>());
            var uow = new UnitOfWork(dbContext.Object);
            var employeeLogService = new EmployeeLogService(uow);

            var ex = Assert.ThrowsAsync<ArgumentException>(() => employeeLogService.Create(null));

            Assert.That(ex.Message.Equals("EmployeeLog is null"));
        }
        
        [Test]
        public async Task GetAllEmployeeLogsTest()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            var employeesLogs = new List<EmployeeLog>
            {
                new EmployeeLog(),
                new EmployeeLog(),
                new EmployeeLog()
            };
            dbContext.Setup(p => p.EmployeeLog).ReturnsDbSet(employeesLogs);
            var uow = new UnitOfWork(dbContext.Object);
            var employeeLogService = new EmployeeLogService(uow);
            var result = await employeeLogService.GetAllEmployeeLogs();

            Assert.True(result.Count.Equals(3));
        }
    }
}