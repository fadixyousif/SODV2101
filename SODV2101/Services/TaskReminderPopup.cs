using SODV2101.Repositories;
using TaskStatus = SODV2101.Enums.TaskStatus;
using Task = SODV2101.Models.Task;

namespace SODV2101.Services
{
    // Service to show task reminders as a single pop-up notification
    internal static class TaskReminderPopup
    {
        // Entrypoint: check tasks and show a single aggregated reminder popup
        public static void CheckReminders()
        {
            // Gather reminder lines
            var lines = GetReminderLines();

            // Show popup if there are any reminders
            if (lines.Count == 0) return;

            // show popup with all reminders
            ShowPopup("Upcoming Tasks Reminder", string.Join("\n", lines));
        }

        // Build the list of reminder lines for tasks due today or in next 2 days
        private static List<string> GetReminderLines()
        {
            var lines = new List<string>();
            foreach (var task in TaskRepository.Tasks)
            {
                if (!ShouldRemind(task)) continue;

                var line = BuildPopData(task);
                if (!string.IsNullOrWhiteSpace(line))
                    lines.Add(line!);
            }
            return lines;
        }

        // Create a single reminder line for a task
        private static string? BuildPopData(Task task)
        {
            var daysLeft = (task.DueDate.Date - DateTime.Today).Days;
            return daysLeft switch
            {
                0 => $" '{task.Title}'{(String.IsNullOrEmpty(task.Subject) ? "" : $" ({task.Subject})")} is due today.",
                > 0 and <= 2 => $" '{task.Title}'{(String.IsNullOrEmpty(task.Subject) ? "" : $" ({task.Subject})")} is due in {daysLeft} day(s).",
                _ => null
            };
        }

        // Determine if a task should trigger a reminder
        private static bool ShouldRemind(Task task)
        {
            if (task.Status == TaskStatus.Completed) return false;
            var daysLeft = (task.DueDate.Date - DateTime.Today).Days;
            return daysLeft >= 0 && daysLeft <= 2;
        }

        // Show the aggregated reminder pop-up
        private static void ShowPopup(string title, string body)
        {
            MessageBox.Show(body, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
