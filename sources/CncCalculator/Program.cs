using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace As.Apps
{
    static class Program
    {
#if USE_LOG4NET
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif

        static Program()
        {
            try
            {
                Name = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                Version = typeof(Program).Assembly.GetName().Version;

                try
                {
                    Product =
                        ((System.Reflection.AssemblyProductAttribute)typeof(Program)
                        .Assembly
                        .GetCustomAttributes(typeof(System.Reflection.AssemblyProductAttribute), true)[0])
                        .Product;
                }
                catch (IndexOutOfRangeException)
                {
                    if (log != null) log.Info("This assembly had a problem during build and appears to work. But it cannot be trusted, please rebuild it.");
                }

                if (log != null) log.Info($"Start {Product}: {Name}, {Version}");
            }
            catch (Exception x)
            {
                try
                {
                    if (log != null) log.Fatal("Unhandled exception, bailing out.", x);
#if DEBUG
                    MessageBox.Show($"Unhandled exception: {x}", x.Message, MessageBoxButtons.OK, MessageBoxIcon.Stop);
#endif
                }
                catch { /* never mind */ }
            }
        }

        public static string Name { get; private set; }

        public static Version Version { get; private set; }

        public static string Product { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new As.Apps.Forms.Main());
            }
            catch (Exception x)
            {
                try
                {
                    if (log != null) log.Fatal("Unhandled application exception, bailing out.", x);
#if DEBUG
                    MessageBox.Show($"Unhandled application exception: {x}", x.Message, MessageBoxButtons.OK, MessageBoxIcon.Stop);
#endif
                }
                catch { /* never mind */ }
            }
            finally
            {
                if (log != null) log.Info($"Stop {Name}.{Environment.NewLine}");
            }
        }
    }
}
