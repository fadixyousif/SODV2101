using TaskPriority = SODV2101.Enums.TaskPriority;
using TaskStatus = SODV2101.Enums.TaskStatus;

namespace SODV2101.Models
{
    public class Task
    {
        public DateTime DueDate { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
    }
}
