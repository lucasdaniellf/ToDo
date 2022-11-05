using Microsoft.Data.Sqlite;
using System.Collections;

namespace TaskManager.Repository.Interface
{
    public interface IRepository<T>
    {
        public Task<IEnumerable<T>> GetAllAsync(CancellationToken token);
        public Task<IEnumerable<T>> GetByIdAsync(int id, CancellationToken token);
        public Task<int> InsertAsync(T t, CancellationToken token);
        public Task<int> UpdateAsync(T t, CancellationToken token);
        public Task<int> DeleteAsync(T t, CancellationToken token);
    }
}
