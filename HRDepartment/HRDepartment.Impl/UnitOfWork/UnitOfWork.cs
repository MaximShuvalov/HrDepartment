using System;
using HRDepartment.Core.Context;
using HRDepartment.Core.Repositories;
using HRDepartment.Core.UnitOfWork;
using HRDepartment.Impl.Repositories;
using HRDepartment.Model;

namespace HRDepartment.Impl.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly HrDepartmentContext _context;

        public UnitOfWork(HrDepartmentContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public IRepository<T> GetRepositories<T>() where T : class
        {
            if (typeof(T) == typeof(Department))
                return new DepartmentRepositories(_context) as IRepository<T>;
            else if (typeof(T) == typeof(Employee))
                return new EmployeeRepositories(_context) as IRepository<T>;
            else if (typeof(T) == typeof(EmployeeLog))
                return new EmployeeLogRepository(_context) as IRepository<T>;
            throw new Exception();
        }

        private bool _disposed = false;

        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                this._disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}