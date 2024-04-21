namespace TaskManager.API.Dtos
{
    public class CreateTaskDetailDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsComplete { get; set; }
    }
}
