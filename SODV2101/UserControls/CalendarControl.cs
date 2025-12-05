using System.ComponentModel;
using Task = SODV2101.Models.Task;
using SODV2101.Repositories;

namespace SODV2101
{
    public class CalendarControl : UserControl
    {
        // UI Components
        private Label lblCalendarTitle;
        private MonthCalendar monthCalendar;
        private DataGridView dgvSchedule;
        private BindingList<Task> scheduledTasks;

        // Constructor for CalendarControl
        public CalendarControl()
        {
            InitializeComponent();
            // subscribe to task changes so calendar updates on add/delete/complete
            TaskRepository.TaskChanged += OnTasksChanged;
            InitCurrentSchedule();
        }

        // Dispose pattern to unsubscribe from events
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                TaskRepository.TaskChanged -= OnTasksChanged;
            }
            base.Dispose(disposing);
        }

        // Event handler for task changes
        private void OnTasksChanged()
        {
            // reload based on current selection
            LoadTasksForSelectedDate();
        }

        // Initialize current day's schedule
        public void InitCurrentSchedule()
        {
            // clear existing tasks
            scheduledTasks.Clear();

            // get today's date without time component
            DateTime now = DateTime.Now.Date;

            // load tasks for today
            foreach (var task in TaskRepository.GetTasksDate(now))
            {
                scheduledTasks.Add(task);
            }
        }

        // Load tasks for the selected date in the calendar
        public void LoadTasksForSelectedDate()
        {
            // get selected date
            DateTime selectedDate = monthCalendar.SelectionStart.Date;

            // clear existing tasks
            scheduledTasks.Clear();

            // load tasks for selected date
            foreach (var task in TaskRepository.GetTasksDate(selectedDate))
            {
                // add tasks for the selected date
                scheduledTasks.Add(task);
            }

            // refresh the DataGridView
            dgvSchedule.Refresh();
        }

        // Initialize UI components
        private void InitializeComponent()
        {
            // make dock fill and light background
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(246, 247, 251);

            // main panel to hold all controls
            var mainPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };

            // Calendar title label
            lblCalendarTitle = new Label
            {
                Text = "Study Schedule",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 15)
            };

            // MonthCalendar setup
            monthCalendar = new MonthCalendar
            {
                Location = new Point(20, 50),
                MaxSelectionCount = 31
            };

            // Event handler for date selection
            monthCalendar.DateSelected += (s, e) => LoadTasksForSelectedDate();

            // DataGridView setup
            scheduledTasks = new BindingList<Task>();

            // DataGridView setup
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

            // Define DataGridView columns
            dgvSchedule.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DueDate", HeaderText = "Date", DefaultCellStyle = new DataGridViewCellStyle { Format = "d" } });
            dgvSchedule.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Title", HeaderText = "Title" });
            dgvSchedule.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Subject", HeaderText = "Course Name" });
            dgvSchedule.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Status", HeaderText = "Status" });
            dgvSchedule.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Priority", HeaderText = "Priority" });

            // Add controls to main panel
            mainPanel.Controls.Add(lblCalendarTitle);
            mainPanel.Controls.Add(monthCalendar);
            mainPanel.Controls.Add(dgvSchedule);

            // Add main panel to UserControl
            this.Controls.Add(mainPanel);

            // Handle resizing to adjust DataGridView size
            this.Resize += (s, e) =>
            {
                dgvSchedule.Size = new Size(this.Width - 60, this.Height - 280);
            };
        }
    }
}
