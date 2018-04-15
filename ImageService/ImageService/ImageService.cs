using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ImageService.Server;
using ImageService.Controller;
using ImageService.Model;
using ImageService.Logging;
using ImageService.Logging.Model;
using System.Configuration;
using ImageService.Infrastructure;

namespace ImageService
{
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

    public partial class ImageService : ServiceBase
    {
        
        private System.ComponentModel.IContainer components = null;
        

        private ImageServer m_imageServer;          // The Image Server
    
        private EventLog eventLog;
        private int eventID = 1;

        // ImageService constructor
        public ImageService(string[] args)
        {
            // init components (eventLog)
            InitializeComponent();

            // in case of parameters for launch
            string eventSourceName = "MySource";
            string logName = "MyNewLog";
            if (args.Count() > 0)
            {
                eventSourceName = args[0];
            }
            if (args.Count() > 1)
            {
                logName = args[1];
            }
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    eventSourceName, logName);
            }
            // assign parameters to log after init
            eventLog.Source = eventSourceName;
            eventLog.Log = logName;

            // init loggingModel
            loggingModel = new LoggingService();
            // subscribe our main Service to the LoggingService
            loggingModel.MessageRecieved += OnMsg;

        }

        [DllImport("advapi32.dll", SetLastError = true)] // set service function helper
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus); // set service func
        

        // Here You will Use the App Config!
        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // write in the log
            eventLog.WriteEntry("Starting ImageService..");

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            eventLog.WriteEntry("Service Running.");
        }

        protected override void OnContinue()
        {
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_CONTINUE_PENDING;
            eventLog.WriteEntry("Service Continued.");
          
        }
        protected override void OnStop()
        {
            // Update the service state to Stop Pending.  
            ServiceStatus serviceStatusStopPending = new ServiceStatus();
            serviceStatusStopPending.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatusStopPending);

            // stopping logic here

            eventLog.WriteEntry("Service Stop-Pending.");




            // Now update service state to Stopped.
            ServiceStatus serviceStatusStopped = new ServiceStatus();
            serviceStatusStopped.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatusStopped);
            eventLog.WriteEntry("Service Stopped.");
        }


        // Logging Logic here
        public void OnMsg(Object sender, MessageRecievedEventArgs eventArgs)
        {
            switch(eventArgs.Status)
            {
                case MessageTypeEnum.INFO:
                    eventLog.WriteEntry(eventArgs.Message, EventLogEntryType.Information, eventID++);
                    break;
                case MessageTypeEnum.WARNING:
                    eventLog.WriteEntry(eventArgs.Message, EventLogEntryType.Warning, eventID++);
                    break;
                case MessageTypeEnum.FAIL:
                    eventLog.WriteEntry(eventArgs.Message, EventLogEntryType.FailureAudit, eventID++);
                    break;
            }
        }
    }
}
