using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
using Status = SODV2101.Enums.TaskStatus;
using TaskPriority = SODV2101.Enums.TaskPriority;
using Task = SODV2101.Models.Task;

namespace SODV2101.Repositories
{
    // Repository class for managing tasks
    internal static class TaskRepository
    {

        private readonly static string _conn = ConfigurationManager.ConnectionStrings["TaskDB"].ConnectionString;

        // public variable to hold the list of tasks
        public static List<Task> Tasks = GetAllTasks();

        // Event raised when tasks change (add/remove/update)
        public static event Action? TaskChanged;

        // Adds a new task to the repository
        public static Task AddTask(Task task)
        {
            // validate input
            if (task == null) return null;

            // query to insert task
            const string query = @"INSERT INTO Tasks (DueDate, Title, Subject, Description, Status, Priority) " +
                                 "VALUES (@DueDate, @Title, @Subject, @Description, @Status, @Priority); " +
                                 "SELECT SCOPE_IDENTITY();"; 
            using (SqlConnection conn = new SqlConnection(_conn))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DueDate", task.DueDate);
                    cmd.Parameters.AddWithValue("@Title", task.Title);
                    cmd.Parameters.AddWithValue("@Subject", task.Subject);
                    cmd.Parameters.AddWithValue("@Description", task.Description);
                    cmd.Parameters.AddWithValue("@Status", (int)task.Status);
                    cmd.Parameters.AddWithValue("@Priority", (int)task.Priority);
                    conn.Open();
                    
                    // Execute the command and get the inserted Id
                    var result = cmd.ExecuteScalar();
                    // Set the Id of the task
                    if (result != null)
                    {
                        task.Id = Convert.ToInt32(result);
                    }

                    // add task to the list
                    Tasks.Add(task);
                }
            }

            // Notify subscribers
            TaskChanged?.Invoke();
            return task;
        }

        // Retrieves all tasks from the data source
        public static List<Task> GetAllTasks()
        {
            List<Task> tasksData = new List<Task>();

            //query to get all tasks

            const string query = "SELECT * FROM Tasks";

            // Connect to the database and retrieve tasks
            using (var conn = new SqlConnection(_conn))
            {
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, conn))
                {
                    // construct a DataTable to hold the data
                    DataTable dt = new DataTable();

                    // fill the DataTable
                    dataAdapter.Fill(dt);

                    // loop through each row and map to Task object
                    foreach (DataRow row in dt.Rows)
                    {
                        // map DataRow to Task object
                        Task task = new Task
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            DueDate = Convert.ToDateTime(row["DueDate"]),
                            Title = row["Title"].ToString() ?? string.Empty,
                            Subject = row["Subject"].ToString() ?? string.Empty,
                            Description = row["Description"].ToString() ?? string.Empty,
                            Status = (Status)Convert.ToInt32(row["Status"]),
                            Priority = (TaskPriority)Convert.ToInt32(row["Priority"])
                        };

                        // add task to the list
                        tasksData.Add(task);
                    }
                }
            }

            // return the list of tasks
            return tasksData;
        }

        // Removes a task from the repository
        public static bool RemoveTask(Task task)
        {
            const string query = "DELETE FROM Tasks WHERE Id = @Id";
            using (SqlConnection conn = new SqlConnection(_conn))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameter to prevent SQL injection
                    cmd.Parameters.AddWithValue("@Id", task.Id);

                    // Execute the delete command
                    conn.Open();

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Tasks.Remove(task);
                        TaskChanged?.Invoke();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

        }

        // Marks a task as completed
        public static bool CompleteTask(int id)
        {
            const string query = "UPDATE Tasks SET Status = @Status WHERE Id = @Id";

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@Status", (int)Status.Completed);
                    cmd.Parameters.AddWithValue("@Id", id);

                    // Execute the update command
                    conn.Open();

                    // Check if any row was affected
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // If update was successful update the local list and notify subscribers
                    if (rowsAffected > 0)
                    {
                        // Update the task in the local list
                        var task = Tasks.FirstOrDefault(t => t.Id == id);
                        if (task != null)
                        {
                            task.Status = Status.Completed;
                        }
                        TaskChanged?.Invoke();
                        return true;
                    }
                    // If no rows were affected, return false
                    else
                    {
                        return false;
                    }
                }
            }
        }

        // Retrieves tasks due on a specific date
        public static List<Task> GetTasksDate(DateTime date)
        {
            // List to hold tasks due on the specified date
            List<Task> tasksOnDate = new List<Task>();

            // Query to select tasks with DueDate matching the specified date
            const string query = "SELECT * FROM Tasks WHERE DueDate = @DueDate";

            // Connect to the database and execute the query
            using (var conn = new SqlConnection(_conn))
            {
                // Create and configure the SQL command
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, conn))
                {
                    // Add parameter to prevent SQL injection
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@DueDate", date.Date);
                    
                    // Construct a DataTable to hold the query results
                    DataTable dt = new DataTable();
                    
                    // Fill the DataTable with the results
                    dataAdapter.Fill(dt);
                    
                    // Loop through each row in the DataTable
                    foreach (DataRow row in dt.Rows)
                    {
                        // Map DataRow to Task object
                        Task task = new Task
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            DueDate = Convert.ToDateTime(row["DueDate"]),
                            Title = row["Title"].ToString() ?? string.Empty,
                            Subject = row["Subject"].ToString() ?? string.Empty,
                            Description = row["Description"].ToString() ?? string.Empty,
                            Status = (Status)Convert.ToInt32(row["Status"]),
                            Priority = (TaskPriority)Convert.ToInt32(row["Priority"])
                        };
                        // Add the task to the list
                        tasksOnDate.Add(task);
                    }
                }
            }

            // Return the list of tasks due on the specified date
            return tasksOnDate;
        }
    }
}
