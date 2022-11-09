using Microsoft.AspNetCore.SignalR;
using TaskManager.Model.ToDo;
using TaskManager.Repository.Interface;
using TaskManager.Services.DTO;
using TaskManager.Services.Hubs;
using TaskManager.UnitOfWork;

namespace TaskManager.Services
{
    public class ToDoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ToDo> _repository;
        private readonly IHubContext<ToDoHub, IToDoHub> _context;
        public ToDoService(IRepository<ToDo> repository, IUnitOfWork unitOfWork, IHubContext<ToDoHub, IToDoHub> context)
        {
            _context = context;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<QueryToDo>> GetToDo(CancellationToken token)
        {
            _unitOfWork.Begin();
            IEnumerable<QueryToDo> toDos = (from todo in await _repository.GetAllAsync(token)
                                           select MapQueryToDo(todo)).ToList();
            
            return toDos;
        }

        public async Task<IEnumerable<QueryToDo>>GetToDoById(int id, CancellationToken token)
        {
            _unitOfWork.Begin();
            IEnumerable<QueryToDo> toDos = (from todo in await _repository.GetByIdAsync(id, token)
                                           select MapQueryToDo(todo)).ToList();
            return toDos;
        }

        public async Task<bool> DeleteToDo(int id, CancellationToken token)
        {
            int rows = 0;
            _unitOfWork.Begin();
            IEnumerable<ToDo> toDos = await _repository.GetByIdAsync(id, token);

            if (toDos.Any())
            {
                ToDo todo = toDos.First();
                todo.DeleteToDo();
                rows = await _repository.DeleteAsync(todo, token);

                if(rows > 0)
                {
                    await _context.Clients.Groups("ToDo").NotifyToDoDeleted(todo);
                }
            }

            //SendCommands

            return rows > 0;
        }

        public async Task<IEnumerable<QueryToDo>> InsertToDo(MutateToDo dto, CancellationToken token)
        {
            _unitOfWork.Begin();
            int id = await _repository.InsertAsync(MapMutateToDo(dto), token);

            IEnumerable<ToDo> newToDo = (from todo in await _repository.GetByIdAsync(id, token)
                                             select todo).ToList();

            if (newToDo.Any())
            {
                await _context.Clients.Groups("ToDo").NotifyToDoCreated(newToDo.First());
            }
            //SendCommands
            
            return newToDo.Select(x => MapQueryToDo(x)); ;
        }

        public async Task<bool> ChangeStatusToDo(int id, CancellationToken token)
        {
            int row = 0;
            _unitOfWork.Begin();
            IEnumerable<ToDo> toDos = await _repository.GetByIdAsync(id, token);

            if (toDos.Any())
            {
                ToDo todo = toDos.First();
                todo.ChangeToDoStatus();
                row = await _repository.UpdateAsync(todo, token);

                if(row > 0)
                {
                    await _context.Clients.Groups("ToDo").NotifyStatusChanged(todo);
                }
            }

            return row > 0;
        }

        public async Task<bool> UpdateToDo(int id, MutateToDo dto, CancellationToken token)
        {
            int row = 0;
            _unitOfWork.Begin();
            IEnumerable<ToDo> toDos = await _repository.GetByIdAsync(id, token);

            if (toDos.Any())
            {
                ToDo todo = toDos.First();
                todo.UpdateTitle(dto.Title);
                todo.UpdateDescription(dto.Description);
                row = await _repository.UpdateAsync(todo, token);

                if (row > 0)
                {
                    await _context.Clients.Groups("ToDo").NotifyToDoUpdated(todo);
                }
            }
            return row > 0;
        }

        private QueryToDo MapQueryToDo(ToDo toDo)
        {
            return new QueryToDo(toDo.Id, toDo.Title, toDo.Description, toDo.IsDone);
        }

        private ToDo MapMutateToDo(MutateToDo dto)
        {
            return ToDo.CreateToDoTask(dto.Title, dto.Description);
        }
    }
}
