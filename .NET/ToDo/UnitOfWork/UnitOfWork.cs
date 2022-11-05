using Microsoft.Data.Sqlite;
using TaskManager.Repository;
namespace TaskManager.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly DbContext _context;
        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public void Begin(bool beginTransaction)
        {
            _context.Connection.Open();
            if (beginTransaction)
            {
                _context.Transaction = _context.Connection.BeginTransaction();
            }
        }

        public bool CommitAsync()
        {
            var success = false;
            try
            {
                _context.Transaction?.Commit();
                DisposeConnection();
                success = true;

            }
            catch (SqliteException e)
            {
                Console.WriteLine(e.Message);
            }

            return success;
        }

        public void RollbackAsync()
        {
            _context.Transaction?.Rollback();
            DisposeConnection();
        }

        private void DisposeConnection()
        {
            _context.Transaction?.Dispose();
            _context.Connection.Dispose();
           
        }
    }
}
