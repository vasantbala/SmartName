using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SmartNamePlate.PCCompanion
{
    public partial class SmartNamePlateService : ServiceBase
    {
        private readonly string EVENT_LOG_SOURCE = "SmartNamePlate";
        private readonly string EVENT_LOG_LOGNAME = "SmartNamePlateLog";
        private readonly string HUB_URL;
        private WindowsStatusHubClient hubClient;

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };

        public SmartNamePlateService()
        {
            InitializeComponent();

            HUB_URL = System.Configuration.ConfigurationManager.AppSettings["hubUrl"];

            if (!EventLog.SourceExists(EVENT_LOG_SOURCE))
            {
                EventLog.CreateEventSource(
                    EVENT_LOG_SOURCE, EVENT_LOG_LOGNAME);
            }
            eventLog.Source = EVENT_LOG_SOURCE;
            eventLog.Log = EVENT_LOG_LOGNAME;
        }

        

        protected async override void OnStart(string[] args)
        {
            eventLog.WriteEntry("Smart Name Plate Service Started.");
            Logger.WriteLog("SNP.Service","Smart Name Plate Service Started.");
            // Update the service state to Start Pending.
            //ServiceStatus serviceStatus = new ServiceStatus();
            //serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            //serviceStatus.dwWaitHint = 100000;
            //SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            hubClient = new WindowsStatusHubClient(HUB_URL);
            bool isConnected = await hubClient.Connect();

            if (isConnected)
            {
                Logger.WriteLog("SNP.Service", "Connection to hub complete.");
            }
            else
            {
                Logger.WriteLog("SNP.Service", "Connection to hub failed.");
            }

            // Update the service state to Running.
            //serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            //SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnStop()
        {
            eventLog.WriteEntry("Smart Name Plate Service Stopped.");
            Logger.WriteLog("SNP.Service", "Smart Name Plate Service Stopped.");
        }

        protected async override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            string logEntry = string.Format("Session Changed: {0}", changeDescription.Reason.ToString());
            eventLog.WriteEntry(logEntry);
            Logger.WriteLog("SNP.Service", logEntry);
            await hubClient.Send(logEntry);
        }
    }
}
