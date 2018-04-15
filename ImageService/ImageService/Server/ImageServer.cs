using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    public class ImageServer
    {
        
        public event EventHandler<DirectoryCloseEventArgs> StopHandler;
        // in future
        //public event EventHandler<CommandRecievedEventArgs> HandlerDispatchCommand;

        private IImageController imageController;
        private ILoggingService loggingService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggingService"></param>
        /// <param name="imageServiceModel"></param>
        public ImageServer(ILoggingService loggingService, IImageServiceModel imageServiceModel)
        {
            this.imageController = new ImageController(imageServiceModel);
            this.loggingService = loggingService;
        }

        /// <summary>
        /// This func adds a directory handler according to path given.
        /// </summary>
        /// <param name="path"></param>
        public void AddDirectoryHandler(string path) // path to directory
        {
            string[] extensions = { "*.jpg", "*.png", "*.gif", "*.bmp" };
            IDirectoryHandler directoryHandler = new DirectoryHandler(path, this.imageController, this.loggingService, extensions);
            // in future
            //HandlerDispatchCommand += directoryHandler.OnCommandRecieved;
            StopHandler += directoryHandler.CloseFileWatcher;
            directoryHandler.DirectoryClose += CloseHandler;
            directoryHandler.StartHandlingDirectory();
        }

        /// <summary>
        /// Closes the directory handler when required..
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        public void CloseHandler(object source, DirectoryCloseEventArgs args)
        {
            
            if (source is IDirectoryHandler)
            {
                IDirectoryHandler handler = (IDirectoryHandler)source;
                // future
                //HandlerDispatchCommand -= handler.OnCommandRecieved;
            }
        }

        /// <summary>
        /// Closes all the handlers for the imageServer (invoked when service is terminated)..
        /// </summary>
        public void CloseImageServer()
        {
            StopHandler.Invoke(this, new DirectoryCloseEventArgs("*", null));
        }
    }
}
