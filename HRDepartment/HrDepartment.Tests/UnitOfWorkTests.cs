using HRDepartment.Core.Context;
using HRDepartment.Core.Repositories;
using HRDepartment.Impl.UnitOfWork;
using HRDepartment.Model;
using HRDepartment.Model.DataBase;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace HrDepartment.Tests
{
    public class UnitOfWorkTests
    {
        [Test]
        public void GetRepositories_ReturnDepartmentRepository_Test()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            var uow = new UnitOfWork(dbContext.Object);

            var result = uow.GetRepositories<Department>();

            Assert.True(result is IRepository<Department>);
        }

        [Test]
        public void GetRepositories_ReturnEmployeeRepository_Test()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            var uow = new UnitOfWork(dbContext.Object);

            var result = uow.GetRepositories<Employee>();

            Assert.True(result is IRepository<Employee>);
        }

        [Test]
        public void GetRepositories_ReturnEmployeeLogRepository_Test()
        {
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            var uow = new UnitOfWork(dbContext.Object);

            var result = uow.GetRepositories<EmployeeLog>();

            Assert.True(result is IRepository<EmployeeLog>);
        }

        [Test]
        public void CommitTest()
        {
            int countSavesChanges = 0;
            var dbContext = new Mock<HrDepartmentContext>(new Mock<DbContextOptions<HrDepartmentContext>>().Object);
            dbContext.Setup(p => p.SaveChanges()).Callback(() => countSavesChanges++);
            var uow = new UnitOfWork(dbContext.Object);

            uow.Commit();

            Assert.True(countSavesChanges.Equals(1));
        }
    }
}