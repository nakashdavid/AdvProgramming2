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

        public ImageServer(ILoggingService loggingService, IImageServiceModel imageServiceModel)
        {
            this.imageController = new ImageController(imageServiceModel);
            this.loggingService = loggingService;
        }

        public void AddDirectoryHandler(string path) // path to directory
        {
            string[] extensions = { "*.jpg", "*.png", "*.gif", "*.bmp" };
            IDirectoryHandler directoryHandler = new DirectoryHandler(path, this.imageController, this.loggingService, extensions);
            //HandlerDispatchCommand += directoryHandler.OnCommandRecieved;
            StopHandler += directoryHandler.CloseFileWatcher;
            directoryHandler.DirectoryClose += CloseHandler;
            directoryHandler.StartHandlingDirectory();
        }

        public void CloseHandler(object sender, DirectoryCloseEventArgs args)
        {
            //args.DirectoryPath
            if (sender is IDirectoryHandler)
            {
                IDirectoryHandler handler = (IDirectoryHandler)sender;
                //HandlerDispatchCommand -= handler.OnCommandRecieved;
            }
        }

        public void CloseAll()
        {
            StopHandler.Invoke(this, new DirectoryCloseEventArgs("*", null));
        }
    }
}
