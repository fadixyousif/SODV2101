using System.ComponentModel;
using Task = SODV2101.Models.Task;
using TaskStatus = SODV2101.Enums.TaskStatus;
using TaskPriority = SODV2101.Enums.TaskPriority;

namespace SODV2101
{
    public class DashboardControl : UserControl
    {
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

        private Panel panelCalendar;
        private Label lblCalendarTitle;
        private MonthCalendar monthCalendar;

        public DashboardControl()
        {
            InitializeComponent();
            PopulateSampleData();
        }

        private void InitializeComponent()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(246, 247, 251);

            // Progress
            panelProgress = CreateCardPanel();
            panelProgress.Location = new Point(15, 15);
            panelProgress.Size = new Size(330, 220);

            lblProgressTitle = new Label
            {
                Text = "Your Progress",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 15)
            };

            lblTasksDueToday = new Label { Text = "Tasks due today", Location = new Point(20, 50), AutoSize = true, Font = new Font("Segoe UI", 9) };
            lblTasksDueTodayValue = new Label { Text = "2", AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), Location = new Point(260, 50) };
            lblTasksCompletedWeek = new Label { Text = "Tasks completed this week", Location = new Point(20, 75), AutoSize = true, Font = new Font("Segoe UI", 9) };
            lblTasksCompletedWeekValue = new Label { Text = "5", AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), Location = new Point(260, 75) };
            lblOverdueTasks = new Label { Text = "Overdue tasks", Location = new Point(20, 100), AutoSize = true, Font = new Font("Segoe UI", 9) };
            lblOverdueTasksValue = new Label { Text = "1", AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), Location = new Point(260, 100) };

            lblTotalTasks = new Label { Text = "Total tasks", Location = new Point(20, 125), AutoSize = true, Font = new Font("Segoe UI", 9) };
            lblTotalTasksValue = new Label { Text = "0", AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), Location = new Point(260, 125) };

            panelProgress.Controls.Add(lblProgressTitle);
            panelProgress.Controls.Add(lblTasksDueToday);
            panelProgress.Controls.Add(lblTasksDueTodayValue);
            panelProgress.Controls.Add(lblTasksCompletedWeek);
            panelProgress.Controls.Add(lblTasksCompletedWeekValue);
            panelProgress.Controls.Add(lblOverdueTasks);
            panelProgress.Controls.Add(lblOverdueTasksValue);
            panelProgress.Controls.Add(lblTotalTasks);
            panelProgress.Controls.Add(lblTotalTasksValue);

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

            this.Controls.Add(panelProgress);
            this.Controls.Add(panelCalendar);
            this.Controls.Add(panelUpcoming);

            this.Resize += (s, e) =>
            {
                panelUpcoming.Size = new Size(this.Width - 30, this.Height - 265);
                dgvUpcoming.Size = new Size(panelUpcoming.Width - 40, panelUpcoming.Height - 60);
            };
        }

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

        private void PopulateSampleData()
        {
            upcomingTasks.Clear();
            upcomingTasks.Add(new Task { DueDate = DateTime.Parse("Nov 30, 2023"), Title = "Submit proposal", Subject = "KIN 231", Status = TaskStatus.InProgress, Priority = TaskPriority.High });
            upcomingTasks.Add(new Task { DueDate = DateTime.Parse("Dec 1, 2023"), Title = "Read Chapter 4", Subject = "CHEM 115", Status = TaskStatus.Pending, Priority = TaskPriority.Medium });
            upcomingTasks.Add(new Task { DueDate = DateTime.Parse("Dec 3, 2023"), Title = "Plan presentation", Subject = "PSYC 121", Status = TaskStatus.Pending, Priority = TaskPriority.Low });

            lblTotalTasksValue.Text = upcomingTasks.Count.ToString();
        }
    }
}
