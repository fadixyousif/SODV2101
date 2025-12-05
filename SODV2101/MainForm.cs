namespace SODV2101
{
    public partial class MainForm : Form
    {
        // The Top nav
        private Panel topNavPanel;
        private Label lblAppTitle;
        private LinkLabel lnkDashboard;
        private LinkLabel lnkTasks;
        private LinkLabel lnkCalendar;
        private LinkLabel lnkViewCompleted;

        // The Main content
        private Panel mainContentPanel;
        private DashboardControl dashboardControl;
        private TasksControl tasksControl;
        private CalendarControl calendarControl;

        public MainForm()
        {
            InitializeComponent();
            lnkDashboard_Click(this, EventArgs.Empty); // Show dashboard on startup
        }

        private void InitializeComponent()
        {
            this.Text = "Study Planner";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(246, 247, 251);
            this.ClientSize = new Size(1100, 700);
            this.MinimumSize = new Size(1000, 650);

            // Top Navigation
            topNavPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.White
            };

            lblAppTitle = new Label
            {
                Text = "Task Scheduler",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 15)
            };

            lnkDashboard = new LinkLabel
            {
                Text = "Dashboard",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(130, 17),
                LinkBehavior = LinkBehavior.NeverUnderline
            };
            lnkDashboard.Click += lnkDashboard_Click;

            lnkTasks = new LinkLabel
            {
                Text = "Tasks",
                Font = new Font("Segoe UI", 9),
                AutoSize = true,
                Location = new Point(220, 17),
                LinkBehavior = LinkBehavior.NeverUnderline
            };
            lnkTasks.Click += lnkTasks_Click;

            lnkCalendar = new LinkLabel
            {
                Text = "Calendar",
                Font = new Font("Segoe UI", 9),
                AutoSize = true,
                Location = new Point(280, 17),
                LinkBehavior = LinkBehavior.NeverUnderline
            };
            lnkCalendar.Click += lnkCalendar_Click;

            lnkViewCompleted = new LinkLabel
            {
                Text = "View Completed",
                Font = new Font("Segoe UI", 9),
                AutoSize = true,
                LinkBehavior = LinkBehavior.NeverUnderline,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(this.ClientSize.Width - 120, 17)
            };

            topNavPanel.Controls.Add(lblAppTitle);
            topNavPanel.Controls.Add(lnkDashboard);
            topNavPanel.Controls.Add(lnkTasks);
            topNavPanel.Controls.Add(lnkCalendar);
            topNavPanel.Controls.Add(lnkViewCompleted);

            this.Resize += (s, e) =>
            {
                lnkViewCompleted.Location = new Point(this.ClientSize.Width - 120, 17);
            };

            // Main Content Panel
            mainContentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(246, 247, 251)
            };

            // User Controls
            dashboardControl = new DashboardControl { Dock = DockStyle.Fill };
            tasksControl = new TasksControl { Dock = DockStyle.Fill };
            calendarControl = new CalendarControl { Dock = DockStyle.Fill };

            mainContentPanel.Controls.Add(dashboardControl);
            mainContentPanel.Controls.Add(tasksControl);
            mainContentPanel.Controls.Add(calendarControl);

            // ADD TO FORM 
            this.Controls.Add(mainContentPanel);
            this.Controls.Add(topNavPanel);
        }

        private void lnkDashboard_Click(object sender, EventArgs e)
        {
            dashboardControl.BringToFront();
            UpdateLinkFonts(lnkDashboard);
        }

        private void lnkTasks_Click(object sender, EventArgs e)
        {
            tasksControl.BringToFront();
            UpdateLinkFonts(lnkTasks);
        }

        private void lnkCalendar_Click(object sender, EventArgs e)
        {
            calendarControl.BringToFront();
            UpdateLinkFonts(lnkCalendar);
        }

        private void UpdateLinkFonts(LinkLabel activeLink)
        {
            lnkDashboard.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lnkTasks.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lnkCalendar.Font = new Font("Segoe UI", 9, FontStyle.Regular);

            activeLink.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        }
    }
}
