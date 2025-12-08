using System.ComponentModel;
using Task = SODV2101.Models.Task;
using SODV2101.Repositories;

namespace SODV2101
{
    public class TasksControl : UserControl
    {
        // UI Components
        private Panel panelTasksRight;
        private Label lblTasksTitle;
        private DataGridView dgvTasks;
        private Button btnAddTask;
        private BindingList<Task> tasks;

        // Constructor for TasksControl
        public TasksControl()
        {
            InitializeComponent();

            // keep task list in sync when repository changes
            TaskRepository.TaskChanged += OnTasksChanged;

            GetTasks();
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
            // reload from repository to avoid duplicates or drift
            GetTasks();
        }

        // Load tasks from repository
        public void GetTasks()
        {
            tasks.Clear();
            foreach (var task in TaskRepository.Tasks)
            {
                tasks.Add(task);
            }
        }

        // Initialize UI components
        private void InitializeComponent()
        {
            // make dock fill and light background
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(246, 247, 251);

            // Right Panel - Tasks
            panelTasksRight = CreateCardPanel();
            panelTasksRight.Location = new Point(15, 15);
            panelTasksRight.Dock = DockStyle.Fill;
            panelTasksRight.Margin = new Padding(15);

            // Tasks Title Label
            lblTasksTitle = new Label
            {
                Text = "Tasks",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 15)
            };

            // Add Task Button
            btnAddTask = new Button
            {
                Text = "+ Add Task",
                AutoSize = true,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9),
                BackColor = Color.FromArgb(63, 99, 255),
                ForeColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnAddTask.FlatAppearance.BorderSize = 0;
            btnAddTask.Location = new Point(panelTasksRight.Width - 110, 10);
            btnAddTask.Size = new Size(90, 30);
            btnAddTask.Click += btnAddTask_Click;

            // Tasks DataGridView
            tasks = new BindingList<Task>();


            // DataGridView
            dgvTasks = new DataGridView
            {
                Location = new Point(18, 45),
                Size = new Size(this.Width - 60, this.Height - 100),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoGenerateColumns = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                DataSource = tasks
            };


            // Define DataGridView Columns
            dgvTasks.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DueDate", HeaderText = "Due Date", DefaultCellStyle = new DataGridViewCellStyle { Format = "d" } });
            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Title", HeaderText = "Title" });
            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Subject", HeaderText = "Subject" });
            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Status", HeaderText = "Status" });
            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Priority", HeaderText = "Priority" });

            // Complete Button Column
            var completeButtonColumn = new DataGridViewButtonColumn
            {
                Name = "Complete",
                Text = "Complete",
                UseColumnTextForButtonValue = true,
                HeaderText = "",
                FlatStyle = FlatStyle.Flat,
                DefaultCellStyle = { BackColor = Color.LightGreen, ForeColor = Color.Black }
            };
            dgvTasks.Columns.Add(completeButtonColumn);

            // Delete Button Column
            var deleteButtonColumn = new DataGridViewButtonColumn
            {
                Name = "Delete",
                Text = "Delete",
                UseColumnTextForButtonValue = true,
                HeaderText = "",
                FlatStyle = FlatStyle.Flat,
                DefaultCellStyle = { BackColor = Color.LightCoral, ForeColor = Color.Black }
            };
            dgvTasks.Columns.Add(deleteButtonColumn);


            // Assemble Right Panel
            panelTasksRight.Controls.Add(lblTasksTitle);
            panelTasksRight.Controls.Add(btnAddTask);
            panelTasksRight.Controls.Add(dgvTasks);

            // Add Panels to UserControl
            this.Controls.Add(panelTasksRight);

            // Handle resizing
            this.Resize += (s, e) =>
            {
                btnAddTask.Location = new Point(panelTasksRight.Width - 110, 10);
                dgvTasks.Size = new Size(this.Width - 60, this.Height - 100);
            };
            dgvTasks.CellContentClick += dgvTasks_CellContentClick;
        }

        // Handle DataGridView button clicks
        private void dgvTasks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore header clicks
            if (e.RowIndex < 0) return;

            // Get the task for the clicked row
            var task = tasks[e.RowIndex];

            // Determine which button was clicked
            if (e.ColumnIndex == dgvTasks.Columns["Delete"].Index)
            {
                // Confirm deletion
                if (MessageBox.Show("Are you sure you want to delete this task?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    bool isDeleted = TaskRepository.RemoveTask(task);
                    if (!isDeleted)
                    {
                        MessageBox.Show("Failed to delete the task.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            // Complete button clicked
            else if (e.ColumnIndex == dgvTasks.Columns["Complete"].Index)
            {
                bool isCompleted = TaskRepository.CompleteTask(task.Id);
                if (isCompleted)
                {
                    dgvTasks.Refresh();
                }
                else
                {
                    MessageBox.Show("Failed to complete the task.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Handle Add Task button click
        private void btnAddTask_Click(object sender, EventArgs e)
        {
            // Show Add Task Form
            using (var addTaskForm = new AddTaskForm())
            {
                // Show dialog and add task if OK is returned then add to repository
                if (addTaskForm.ShowDialog() == DialogResult.OK)
                {
                    // Add through repository to trigger events
                    TaskRepository.AddTask(addTaskForm.Task);
                }
            }
        }

        // Create a styled panel for cards
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
