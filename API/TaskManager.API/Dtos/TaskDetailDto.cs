namespace TaskManager.API.Dtos
{
    public class TaskDetailDto
    {
        public long Id { get; set; } = 0;
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsComplete { get; set; }
    }
}
