using System.ComponentModel;
using Task = SODV2101.Models.Task;
using TaskStatus = SODV2101.Enums.TaskStatus;
using TaskPriority = SODV2101.Enums.TaskPriority;

namespace SODV2101
{
    public class CalendarControl : UserControl
    {
        private Label lblCalendarTitle;
        private MonthCalendar monthCalendar;
        private DataGridView dgvSchedule;
        private BindingList<Task> scheduledTasks;

        public CalendarControl()
        {
            InitializeComponent();
            PopulateSampleData();
        }

        private void InitializeComponent()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(246, 247, 251);

            var mainPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };

            lblCalendarTitle = new Label
            {
                Text = "Study Schedule",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 15)
            };

            monthCalendar = new MonthCalendar
            {
                Location = new Point(20, 50),
                MaxSelectionCount = 31
            };

            scheduledTasks = new BindingList<Task>();

            dgvSchedule = new DataGridView
            {
                Location = new Point(20, 230),
                Size = new Size(this.Width - 60, this.Height - 280),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoGenerateColumns = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                DataSource = scheduledTasks
            };
            dgvSchedule.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            dgvSchedule.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DueDate", HeaderText = "Date", DefaultCellStyle = new DataGridViewCellStyle { Format = "d" } });
            dgvSchedule.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Title", HeaderText = "Title" });
            dgvSchedule.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Subject", HeaderText = "Course Name" });
            dgvSchedule.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Status", HeaderText = "Status" });
            dgvSchedule.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Priority", HeaderText = "Priority" });

            mainPanel.Controls.Add(lblCalendarTitle);
            mainPanel.Controls.Add(monthCalendar);
            mainPanel.Controls.Add(dgvSchedule);

            this.Controls.Add(mainPanel);

            this.Resize += (s, e) =>
            {
                dgvSchedule.Size = new Size(this.Width - 60, this.Height - 280);
            };
        }

        private void PopulateSampleData()
        {
            scheduledTasks.Clear();
            scheduledTasks.Add(new Task { DueDate = DateTime.Parse("Nov 30, 2023"), Title = "Submit proposal", Subject = "KIN 231", Status = TaskStatus.InProgress, Priority = TaskPriority.High });
            scheduledTasks.Add(new Task { DueDate = DateTime.Parse("Dec 1, 2023"), Title = "Read Chapter 4", Subject = "CHEM 115", Status = TaskStatus.Pending, Priority = TaskPriority.Medium });
            scheduledTasks.Add(new Task { DueDate = DateTime.Parse("Dec 3, 2023"), Title = "Plan presentation", Subject = "PSYC 121", Status = TaskStatus.Pending, Priority = TaskPriority.Low });
            scheduledTasks.Add(new Task { DueDate = DateTime.Parse("Dec 5, 2023"), Title = "Start final project", Subject = "CHEM 115", Status = TaskStatus.Pending, Priority = TaskPriority.Medium });
        }
    }
}
