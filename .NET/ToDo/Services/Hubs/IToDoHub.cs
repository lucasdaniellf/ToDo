using TaskManager.Model.ToDo;

namespace TaskManager.Services.Hubs
{
    public interface IToDoHub
    {
        public Task NotifyStatusChanged(int id);
        public Task NotifyToDoUpdated(ToDo t);
        public Task NotifyToDoCreated(ToDo t);
        public  Task NotifyToDoDeleted(ToDo t);
    }
}
