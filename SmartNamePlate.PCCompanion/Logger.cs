using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartNamePlate.PCCompanion
{
    public class Logger
    {
		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void WriteLog(string caller, string message)
        {
			logger.Info(string.Format("{0} - {1}", caller, message));
            //System.IO.File.AppendAllText(LOG_FILE, string.Format("\r\n{0}{1} - {2}", DateTime.Now, caller, message));
        }

    }
}
