
using ImageService.Logging.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        public void Log(object source, MessageRecievedEventArgs args)
        {
            
            // Notify all of the subscribers.
            MessageRecieved(source, args);
        }
    }
}
