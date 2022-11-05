using System.Data;

namespace TaskManager.UnitOfWork
{
    public interface IUnitOfWork
    {
        public void Begin(bool beginTransaction = false);
        public bool CommitAsync();
        public void RollbackAsync();
    }
}
