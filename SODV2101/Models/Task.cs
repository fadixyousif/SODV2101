using TaskPriority = SODV2101.Enums.TaskPriority;
using TaskStatus = SODV2101.Enums.TaskStatus;

namespace SODV2101.Models
{
    // Model class representing a Task
    public class Task
    {
        // Unique identifier for the task
        public int Id { get; set; }

        // Due date of the task
        public DateTime DueDate { get; set; }

        // Title of the task
        public string Title { get; set; }

        // Subject associated with the task
        public string Subject { get; set; }
        // Description of the task
        public string Description { get; set; }

        // Status of the task (e.g., Pending, Completed)
        public TaskStatus Status { get; set; }

        // Priority level of the task (e.g., Low, Medium, High)
        public TaskPriority Priority { get; set; }
    }
}
