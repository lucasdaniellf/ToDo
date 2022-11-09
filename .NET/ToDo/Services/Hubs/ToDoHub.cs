using Microsoft.AspNetCore.SignalR;
using TaskManager.Model.ToDo;

namespace TaskManager.Services.Hubs
{
    public class ToDoHub : Hub<IToDoHub>
    {
        public override Task OnConnectedAsync()
        {
            //Creating groups and assigning connection id of user to that group
            var group = Context.GetHttpContext()?.Request.Query["group"].ToString();
            if (!string.IsNullOrEmpty(group))
            {
                Groups.AddToGroupAsync(Context.ConnectionId, group);
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
        
        public async Task NotifyStatusChanged(ToDo t)
        {
            await Clients.Group("ToDo").NotifyStatusChanged(t);

            //Outra possibilidade:
            //await Clients.OthersInGroup("ToDo").SendAsync(nameof(NotifyStatusChanged), id);

        }

        public async Task NotifyToDoUpdated(ToDo t)
        {
            await Clients.Group("ToDo").NotifyToDoUpdated(t);

        }

        public async Task NotifyToDoCreated(ToDo t)
        {
            await Clients.Group("ToDo").NotifyToDoCreated(t);

        }
        public async Task NotifyToDoDeleted(ToDo t)
        {
            await Clients.Group("ToDo").NotifyToDoDeleted(t);
        }
    }
}
