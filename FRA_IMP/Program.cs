using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WHConsult.Utils.Log4Net;

namespace FRA_IMP
{

    static class Program
    {
        static ILogService logService;


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            logService = new FileLogService(typeof(Program));

            try
            {
                // Update user settings in case of a new version
                if (!Properties.Settings.Default.Upgraded)
                {
                    Properties.Settings.Default.Upgrade();
                    Properties.Settings.Default.Upgraded = true;
                    Properties.Settings.Default.Save();
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
            }
            catch (Exception e) when (!System.Diagnostics.Debugger.IsAttached)  // only in release version, disabled for debugging
            {
                StringWriter writer = new StringWriter();
                writer.WriteLine("Message: " + e.Message);
                writer.WriteLine("Source: " + e.Source);
                writer.WriteLine("StackTrace: " + e.StackTrace);
                writer.WriteLine("TargetSite: " + e.TargetSite.ToString());

                logService.Error(writer.ToString());
                MessageBox.Show("An unexpected error occured, causing FRA_IMP to shut down. Please check 'FRA_IMP.log' under the installation directory for more detailed information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(writer.ToString(), "Error Detail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw (e);
            }
        }
    }

    public static class Env
    {
#if DEBUG
        public static readonly bool Debugging = true;
#else
    public static readonly bool Debugging = false;
#endif
    }
}
