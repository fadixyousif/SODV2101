using System.ComponentModel;
using Task = SODV2101.Models.Task;
using TaskStatus = SODV2101.Enums.TaskStatus;
using SODV2101.Repositories;

namespace SODV2101
{
    public class DashboardControl : UserControl
    {

        // UI Components
        private Panel panelUpcoming;
        private Label lblUpcomingTitle;
        private DataGridView dgvUpcoming;
        private BindingList<Task> upcomingTasks;

        private Panel panelProgress;
        private Label lblProgressTitle;
        private Label lblTasksDueToday;
        private Label lblTasksCompletedWeek;
        private Label lblOverdueTasks;
        private Label lblTotalTasks;
        private Label lblTasksDueTodayValue;
        private Label lblTasksCompletedWeekValue;
        private Label lblOverdueTasksValue;
        private Label lblTotalTasksValue;
        private Label lblCompletion;
        private Label lblCompletionValue;

        private Panel panelCalendar;
        private Label lblCalendarTitle;
        private MonthCalendar monthCalendar;

        // Constructor for DashboardControl
        public DashboardControl()
        {
            InitializeComponent();
            // Subscribe to repository updates
            TaskRepository.TaskChanged += OnTasksChanged;
            InitDashboardData();
        }

        // Unsubscribe from events to prevent memory leaks
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                TaskRepository.TaskChanged -= OnTasksChanged;
            }
            base.Dispose(disposing);
        }

        // Handle task changes
        private void OnTasksChanged()
        {
            // Recompute dashboard when tasks change
            InitDashboardData();
        }

        private void InitDashboardData()
        {
            // get all tasks
            List<Task> tasks = TaskRepository.Tasks;

            // total tasks
            lblTotalTasksValue.Text = tasks.Count.ToString();

            // upcoming tasks in next 7 days

            DateTime today = DateTime.Today;
            DateTime weekFromToday = today.AddDays(7);
            var upcoming = tasks.Where(t => t.DueDate >= today && t.DueDate <= weekFromToday)
                                .OrderBy(t => t.DueDate)
                                .ToList();
            upcomingTasks.Clear();
            foreach (var task in upcoming)
            {
                upcomingTasks.Add(task);
            }

            // tasks due today
            int dueTodayCount = tasks.Count(t => t.DueDate.Date == today);

            lblTasksDueTodayValue.Text = dueTodayCount.ToString();

            // tasks completed this week
            DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            int completedThisWeekCount = tasks.Count(t => t.Status == TaskStatus.Completed && t.DueDate.Date >= startOfWeek && t.DueDate.Date <= today);
            lblTasksCompletedWeekValue.Text = completedThisWeekCount.ToString();

            // overdue tasks
            int overdueCount = tasks.Count(t => t.DueDate.Date < today && t.Status != TaskStatus.Completed);
            lblOverdueTasksValue.Text = overdueCount.ToString();

            // completed and total tasks for completion percentage
            int completedCount = tasks.Count(t => t.Status == TaskStatus.Completed);
            int totalCount = tasks.Count;
            double completionPct = totalCount == 0 ? 0 : (completedCount * 100.0 / totalCount);
            lblCompletionValue.Text = $"{completionPct:F0}%";

            // Refresh DataGridView

            dgvUpcoming.Refresh();
        }

        // Initialize UI components
        private void InitializeComponent()
        {
            // make dock fill and light background
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(246, 247, 251);

            // Progress
            panelProgress = CreateCardPanel();
            panelProgress.Location = new Point(15, 15);
            panelProgress.Size = new Size(330, 220);

            // Progress Labels
            lblProgressTitle = new Label
            {
                Text = "Your Progress",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 15)
            };

            // Stats Labels
            lblTasksDueToday = new Label { Text = "Tasks due today", Location = new Point(20, 50), AutoSize = true, Font = new Font("Segoe UI", 9) };
            lblTasksDueTodayValue = new Label { Text = "0", AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), Location = new Point(260, 50) };
            lblTasksCompletedWeek = new Label { Text = "Tasks completed this week", Location = new Point(20, 75), AutoSize = true, Font = new Font("Segoe UI", 9) };
            lblTasksCompletedWeekValue = new Label { Text = "0", AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), Location = new Point(260, 75) };
            lblOverdueTasks = new Label { Text = "Overdue tasks", Location = new Point(20, 100), AutoSize = true, Font = new Font("Segoe UI", 9) };
            lblOverdueTasksValue = new Label { Text = "0", AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), Location = new Point(260, 100) };

            // Total Tasks
            lblTotalTasks = new Label { Text = "Total tasks", Location = new Point(20, 125), AutoSize = true, Font = new Font("Segoe UI", 9) };
            lblTotalTasksValue = new Label { Text = "0", AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), Location = new Point(260, 125) };

            // Completion Percentage
            lblCompletion = new Label { Text = "Completion", Location = new Point(20, 150), AutoSize = true, Font = new Font("Segoe UI", 9) };
            lblCompletionValue = new Label { Text = "0%", AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), Location = new Point(260, 150) };

            // Add labels to Progress panel
            panelProgress.Controls.Add(lblProgressTitle);
            panelProgress.Controls.Add(lblTasksDueToday);
            panelProgress.Controls.Add(lblTasksDueTodayValue);
            panelProgress.Controls.Add(lblTasksCompletedWeek);
            panelProgress.Controls.Add(lblTasksCompletedWeekValue);
            panelProgress.Controls.Add(lblOverdueTasks);
            panelProgress.Controls.Add(lblOverdueTasksValue);
            panelProgress.Controls.Add(lblTotalTasks);
            panelProgress.Controls.Add(lblTotalTasksValue);
            panelProgress.Controls.Add(lblCompletion);
            panelProgress.Controls.Add(lblCompletionValue);

            // Calendar
            panelCalendar = CreateCardPanel();
            panelCalendar.Location = new Point(360, 15);
            panelCalendar.Size = new Size(330, 220);

            lblCalendarTitle = new Label
            {
                Text = "Calendar",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 15)
            };

            monthCalendar = new MonthCalendar { Location = new Point(15, 40) };

            panelCalendar.Controls.Add(lblCalendarTitle);
            panelCalendar.Controls.Add(monthCalendar);

            // Upcoming Tasks
            panelUpcoming = CreateCardPanel();
            panelUpcoming.Location = new Point(15, 250);
            panelUpcoming.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelUpcoming.Size = new Size(this.Width - 30, this.Height - 265);

            lblUpcomingTitle = new Label
            {
                Text = "Upcoming Tasks",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 15)
            };

            upcomingTasks = new BindingList<Task>();


            // Initialize DataGridView for Upcoming Tasks
            dgvUpcoming = new DataGridView
            {
                Location = new Point(18, 40),
                Size = new Size(panelUpcoming.Width - 40, panelUpcoming.Height - 60),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoGenerateColumns = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                EnableHeadersVisualStyles = false,
                DataSource = upcomingTasks
            };


            dgvUpcoming.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvUpcoming.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvUpcoming.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvUpcoming.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvUpcoming.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);

            dgvUpcoming.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DueDate", HeaderText = "Date", DefaultCellStyle = new DataGridViewCellStyle { Format = "d" } });
            dgvUpcoming.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Title", HeaderText = "Title" });
            dgvUpcoming.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Subject", HeaderText = "Course Name" });
            dgvUpcoming.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Status", HeaderText = "Status" });
            dgvUpcoming.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Priority", HeaderText = "Priority" });

            panelUpcoming.Controls.Add(lblUpcomingTitle);
            panelUpcoming.Controls.Add(dgvUpcoming);

            // Add panels to DashboardControl
            this.Controls.Add(panelProgress);
            this.Controls.Add(panelCalendar);
            this.Controls.Add(panelUpcoming);

            // Handle resizing
            this.Resize += (s, e) =>
            {
                panelUpcoming.Size = new Size(this.Width - 30, this.Height - 265);
                dgvUpcoming.Size = new Size(panelUpcoming.Width - 40, panelUpcoming.Height - 60);
            };
        }

        // Helper method to create styled card panels
        private Panel CreateCardPanel()
        {
            return new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(5),
                Margin = new Padding(10)
            };
        }
    }
}
