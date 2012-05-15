using System;
using System.Windows.Forms;

namespace ZerosTwitterClient
{
    static class Program
    {
        public static ModerationForm ModerationForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ModerationForm = new ModerationForm();
            Application.Run(ModerationForm);
        }
    }
}
