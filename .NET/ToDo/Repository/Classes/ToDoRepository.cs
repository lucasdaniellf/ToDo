using Dapper;
using TaskManager.Model.ToDo;
using TaskManager.Repository.Interface;

namespace TaskManager.Repository.Classes
{
    public class ToDoRepository : IRepository<ToDo>
    {
        private readonly DbContext _context;
        public ToDoRepository(DbContext context)
        {
            _context = context;     
        }

        public async Task<int> DeleteAsync(ToDo todo, CancellationToken token)
        {
            var sql = @"update ToDo set isActive = 0 where Id = @ID";
            int rows = await _context.Connection.ExecuteAsync(new CommandDefinition(commandText: sql, 
                                                                                      parameters: todo, 
                                                                                      transaction: _context.Transaction, 
                                                                                      commandType: System.Data.CommandType.Text, 
                                                                                      cancellationToken: token));
            return rows;
        }

        public async Task<IEnumerable<ToDo>> GetAllAsync(CancellationToken token)
        {
            var sql = @"select * from ToDo where IsActive = 1";
            IEnumerable<ToDo> toDos = await _context.Connection.QueryAsync<ToDo>(new CommandDefinition(commandText: sql,
                                                                                                        transaction: _context.Transaction,
                                                                                                        commandType: System.Data.CommandType.Text,
                                                                                                        cancellationToken: token));
            return toDos;
        }

        public async Task<IEnumerable<ToDo>> GetByIdAsync(int id, CancellationToken token)
        {
            var sql = @"select * from ToDo where Id = @Id and IsActive = 1";
            IEnumerable<ToDo> toDos = await _context.Connection.QueryAsync<ToDo>(new CommandDefinition(commandText: sql,
                                                                                                        parameters: new { Id = id},
                                                                                                        transaction: _context.Transaction,
                                                                                                        commandType: System.Data.CommandType.Text,
                                                                                                        cancellationToken: token));
            return toDos;
        }

        public async Task<int> InsertAsync(ToDo toDo, CancellationToken token)
        {
            var sql = @"insert into ToDo(Title, Description, IsDone, IsActive) values (@Title, @Description, 0, 1) RETURNING Id";
            int rows = await _context.Connection.ExecuteScalarAsync<int>(new CommandDefinition(commandText: sql,
                                                                                      parameters: new { Title = toDo.Title, Description = toDo.Description } ,
                                                                                      transaction: _context.Transaction,
                                                                                      commandType: System.Data.CommandType.Text,
                                                                                      cancellationToken: token));
            return rows;
        }

        public async Task<int> UpdateAsync(ToDo toDo, CancellationToken token)
        {
            var sql = @"update ToDo set Title = @Title, Description = @Description, IsDone = @IsDone where Id = @Id and IsActive = 1";
            int rows = await _context.Connection.ExecuteAsync(new CommandDefinition(commandText: sql,
                                                                                      parameters: toDo,
                                                                                      transaction: _context.Transaction,
                                                                                      commandType: System.Data.CommandType.Text,
                                                                                      cancellationToken: token));
            return rows;
        }
    }
}
