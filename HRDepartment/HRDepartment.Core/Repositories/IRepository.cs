using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRDepartment.Core.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task Create(T item);
        Task Delete(T item);
        Task<T> Get(long id);
        Task Update(T item);
        Task<List<T>> GetAllAsync();
        List<T> GetAll();
    }
}