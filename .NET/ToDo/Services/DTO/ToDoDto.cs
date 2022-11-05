using TaskManager.Model.ToDo;

namespace TaskManager.Services.DTO
{
    public record QueryToDo(int Id, string Title, string Description, Status IsDone)
    {
    }

    public record MutateToDo(string Title, string Description)
    {
    }
}
