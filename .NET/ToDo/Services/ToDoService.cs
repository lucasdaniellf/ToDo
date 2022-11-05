using TaskManager.Model.ToDo;
using TaskManager.Repository.Interface;
using TaskManager.Services.DTO;
using TaskManager.UnitOfWork;

namespace TaskManager.Services
{
    public class ToDoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ToDo> _repository;
        public ToDoService(IRepository<ToDo> repository, IUnitOfWork unitOfWork)
        {
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
            }

            //SendCommands

            return rows > 0;
        }

        public async Task<IEnumerable<QueryToDo>> InsertToDo(MutateToDo dto, CancellationToken token)
        {
            _unitOfWork.Begin();
            int id = await _repository.InsertAsync(MapMutateToDo(dto), token);

            IEnumerable<QueryToDo> newToDo = (from todo in await _repository.GetByIdAsync(id, token)
                                             select MapQueryToDo(todo)).ToList();


            //SendCommands

            return newToDo;
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
