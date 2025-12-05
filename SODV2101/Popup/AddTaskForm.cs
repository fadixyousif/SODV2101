using SODV2101.Repositories;
using Task = SODV2101.Models.Task;
using TaskPriority = SODV2101.Enums.TaskPriority;
using TaskStatus = SODV2101.Enums.TaskStatus;

namespace SODV2101
{
    public class AddTaskForm : Form
    {
        // UI Components
        private Label lblAddTaskTitle;
        private Label lblTitle;
        private TextBox txtTitle;
        private Label lblDescription;
        private RichTextBox txtDescription;
        private Label lblSubject;
        private TextBox cboSubject;
        private Label lblDueDate;
        private DateTimePicker dtpDueDate;
        private Label lblPriority;
        private ComboBox cboPriority;
        private Button btnCancel;
        private Button btnSave;

        // Public property to access the created task
        public Task Task { get; private set; }

        // Constructor for AddTaskForm
        public AddTaskForm()
        {
            InitializeComponent();
            cboPriority.DataSource = Enum.GetValues(typeof(TaskPriority));
        }

        // Initialize UI components 
        private void InitializeComponent()
        {

            // Form properties
            this.Text = "Add New Task";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.BackColor = Color.FromArgb(246, 247, 251);
            this.ClientSize = new Size(650, 520);
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Main Panel (Card)
            var cardPanel = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(5),
                Margin = new Padding(10),
                Dock = DockStyle.Fill
            };

            // Add Task Title Label
            lblAddTaskTitle = new Label
            {
                Text = "Add Task",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 15)
            };

            // Form Controls
            int leftCol = 35;
            int controlWidth = 580;

            lblTitle = new Label { Text = "Title", Location = new Point(leftCol, 50), AutoSize = true };
            txtTitle = new TextBox { Location = new Point(leftCol, 70), Width = controlWidth };

            lblDescription = new Label { Text = "Description", Location = new Point(leftCol, 105), AutoSize = true };
            txtDescription = new RichTextBox { Location = new Point(leftCol, 125), Width = controlWidth, Height = 120 };

            lblSubject = new Label { Text = "Subject", Location = new Point(leftCol, 255), AutoSize = true };
            cboSubject = new TextBox { Location = new Point(leftCol, 275), Width = controlWidth };

            lblDueDate = new Label { Text = "Due Date", Location = new Point(leftCol, 310), AutoSize = true };
            dtpDueDate = new DateTimePicker { Location = new Point(leftCol, 330), Width = controlWidth };

            lblPriority = new Label { Text = "Priority", Location = new Point(leftCol, 365), AutoSize = true };
            cboPriority = new ComboBox { Location = new Point(leftCol, 385), Width = controlWidth, DropDownStyle = ComboBoxStyle.DropDownList };

            btnCancel = new Button
            {
                Text = "Cancel",
                Width = 130,
                Height = 35,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9),
                Location = new Point(leftCol, 440)
            };
            btnCancel.FlatAppearance.BorderSize = 1;
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            btnSave = new Button
            {
                Text = "Save",
                Width = 130,
                Height = 35,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9),
                BackColor = Color.FromArgb(63, 99, 255),
                ForeColor = Color.White,
                Location = new Point(leftCol + 150, 440)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            // Save button click event
            btnSave.Click += btnSave_Click;

            // Add controls to card panel
            cardPanel.Controls.Add(lblAddTaskTitle);
            cardPanel.Controls.Add(lblTitle);
            cardPanel.Controls.Add(txtTitle);
            cardPanel.Controls.Add(lblDescription);
            cardPanel.Controls.Add(txtDescription);
            cardPanel.Controls.Add(lblSubject);
            cardPanel.Controls.Add(cboSubject);
            cardPanel.Controls.Add(lblDueDate);
            cardPanel.Controls.Add(dtpDueDate);
            cardPanel.Controls.Add(lblPriority);
            cardPanel.Controls.Add(cboPriority);
            cardPanel.Controls.Add(btnCancel);
            cardPanel.Controls.Add(btnSave);

            // Add card panel to form
            this.Controls.Add(cardPanel);
        }

        // Save button click event handler
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please fill in all required fields (Title).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Create new Task object
            Task = new Task
            {
                Title = txtTitle.Text,
                Description = txtDescription.Text,
                Subject = cboSubject.Text,
                DueDate = dtpDueDate.Value,
                Priority = (TaskPriority)cboPriority.SelectedItem,
                Status = TaskStatus.Pending
            };

            // Set dialog result and close form
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
