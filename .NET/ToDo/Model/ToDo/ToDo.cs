namespace TaskManager.Model.ToDo
{
    public class ToDo
    {
        private ToDo() { }

        public int Id { get; private set; }
        public string Title { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public Status IsDone { get; private set; } = Status.FALSE;
        public Status IsActive { get; private set; } = Status.TRUE;


        public static ToDo CreateToDoTask(string title, string description)
        {
            ToDo toDo = new();

            toDo.UpdateTitle(title);
            toDo.UpdateDescription(description);
            
            return toDo;
        }

        public void ChangeToDoStatus()
        {
            IsDone = IsDone == Status.TRUE ? Status.FALSE : Status.TRUE;
        }

        public void DeleteToDo()
        {
            this.IsActive = Status.FALSE;
        }

        public void UpdateTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new Exception("Title cannot be empty");
            }else if (title.Length > 50)
            {
                throw new Exception("Invalid title lenght, must have no more than 50 characters");
            }

            this.Title = title;
        }

        public void UpdateDescription(string description)
        {
            if (description.Length > 300)
            {
                throw new Exception("Invalid description length, must have no more than 300 characters");
            }

            this.Description = description;
        }
    }
}
