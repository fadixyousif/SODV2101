namespace SODV2101
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Set the DataDirectory to the project root for database file access
            var projectRoot = Path.GetFullPath(Path.Combine(Application.StartupPath, "..", "..", ".."));
            AppDomain.CurrentDomain.SetData("DataDirectory", projectRoot);

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}