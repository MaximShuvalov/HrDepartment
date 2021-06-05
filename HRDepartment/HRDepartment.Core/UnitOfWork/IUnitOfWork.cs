using System;
using HRDepartment.Core.Repositories;

namespace HRDepartment.Core.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public void Commit();
        public IRepository<T> GetRepositories<T>() where T : class;
    }
}