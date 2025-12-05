using System.ComponentModel;
using Task = SODV2101.Models.Task;
using TaskStatus = SODV2101.Enums.TaskStatus;
using TaskPriority = SODV2101.Enums.TaskPriority;

namespace SODV2101
{
    public class TasksControl : UserControl
    {
        private Panel panelTasksRight;
        private Label lblTasksTitle;
        private DataGridView dgvTasks;
        private Button btnAddTask;
        private BindingList<Task> tasks;

        public TasksControl()
        {
            InitializeComponent();
            PopulateSampleData();
        }

        private void InitializeComponent()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(246, 247, 251);

            panelTasksRight = CreateCardPanel();
            panelTasksRight.Location = new Point(15, 15);
            panelTasksRight.Dock = DockStyle.Fill;
            panelTasksRight.Margin = new Padding(15);

            lblTasksTitle = new Label
            {
                Text = "Tasks",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 15)
            };

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

            tasks = new BindingList<Task>();

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

            dgvTasks.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DueDate", HeaderText = "Due Date", DefaultCellStyle = new DataGridViewCellStyle { Format = "d" } });
            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Title", HeaderText = "Title" });
            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Subject", HeaderText = "Subject" });
            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Status", HeaderText = "Status" });
            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Priority", HeaderText = "Priority" });

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

            panelTasksRight.Controls.Add(lblTasksTitle);
            panelTasksRight.Controls.Add(btnAddTask);
            panelTasksRight.Controls.Add(dgvTasks);

            this.Controls.Add(panelTasksRight);

            this.Resize += (s, e) =>
            {
                btnAddTask.Location = new Point(panelTasksRight.Width - 110, 10);
                dgvTasks.Size = new Size(this.Width - 60, this.Height - 100);
            };
            dgvTasks.CellContentClick += dgvTasks_CellContentClick;
        }

        private void dgvTasks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var task = tasks[e.RowIndex];

            if (e.ColumnIndex == dgvTasks.Columns["Delete"].Index)
            {
                if (MessageBox.Show("Are you sure you want to delete this task?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    tasks.RemoveAt(e.RowIndex);
                }
            }
            else if (e.ColumnIndex == dgvTasks.Columns["Complete"].Index)
            {
                task.Status = TaskStatus.Completed;
                tasks.ResetItem(e.RowIndex);
            }
        }

        private void btnAddTask_Click(object sender, EventArgs e)
        {
            using (var addTaskForm = new AddTaskForm())
            {
                if (addTaskForm.ShowDialog() == DialogResult.OK)
                {
                    tasks.Add(addTaskForm.Task);
                }
            }
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
            tasks.Clear();
            tasks.Add(new Task { DueDate = DateTime.Parse("Nov 30, 2023"), Title = "Submit proposal", Subject = "KIN 231", Status = TaskStatus.InProgress, Priority = TaskPriority.High });
            tasks.Add(new Task { DueDate = DateTime.Parse("Dec 1, 2023"), Title = "Read Chapter 4", Subject = "CHEM 115", Status = TaskStatus.Pending, Priority = TaskPriority.Medium });
            tasks.Add(new Task { DueDate = DateTime.Parse("Dec 3, 2023"), Title = "Plan presentation", Subject = "PSYC 121", Status = TaskStatus.Pending, Priority = TaskPriority.Low });
        }
    }
}
