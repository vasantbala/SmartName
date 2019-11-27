using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartNamePlate.PCCompanion
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {

            if (args.Length == 1 && args[0] == "/s")
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                using (Form startForm = new DebugMain())
                {
                    Application.Run(startForm);
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new SmartNamePlateService()
                };
                ServiceBase.Run(ServicesToRun);
            }
            
        }
    }
}
