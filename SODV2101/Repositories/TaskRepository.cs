using Task = SODV2101.Models.Task;
using Status = SODV2101.Enums.TaskStatus;

namespace SODV2101.Repositories
{
    // Repository class for managing tasks
    internal static class TaskRepository
    {

        // public variable to hold the list of tasks
        public static List<Task> Tasks = GetAllTasks();

        // Event raised when tasks change (add/remove/update)
        public static event Action? TaskChanged;

        public static Task AddTask(Task task)
        {
            Tasks.Add(task);
            // Notify subscribers
            TaskChanged?.Invoke();
            return task;
        }

        // Retrieves all tasks from the data source
        public static List<Task> GetAllTasks()
        {
            return new List<Task>();
        }

        // Removes a task from the repository
        public static void RemoveTask(Task task)
        {
            Tasks.Remove(task);
            TaskChanged?.Invoke();
        }

        // Retrieves a task by its ID
        public static Task GetTaskById(int id)
        {
            return Tasks.FirstOrDefault(t => t.Id == id);
        }

        // Marks a task as completed
        public static bool CompleteTask(int id)
        {
            var task = GetTaskById(id);
            if (task != null)
            {
                task.Status = Status.Completed;
                TaskChanged?.Invoke();
                return true;
            }
            return false;
        }

        // Retrieves tasks due on a specific date
        public static List<Task> GetTasksDate(DateTime date)
        {
            return Tasks.Where(t => t.DueDate.Date == date.Date).ToList();
        }
    }
}
