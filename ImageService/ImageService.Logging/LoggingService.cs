
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

        public void Log(string message, MessageTypeEnum type)
        {
            // Create a MessageReceivedEventArgs.
            MessageRecievedEventArgs msgReceived = new MessageRecievedEventArgs
            {
                Status = type,
                Message = message
            };
            // Notify all of the subscribers.
            MessageRecieved(this, msgReceived);
        }
    }
}
