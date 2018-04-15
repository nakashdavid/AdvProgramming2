﻿using ImageService.Controller;
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
        #region Properties
        public event EventHandler<CommandRecievedEventArgs> OnServerCommand;
        public event EventHandler<DirectoryCloseEventArgs> OnStopHandler;
        #endregion

        #region Members
        private IImageController imageController;
        private ILoggingService loggingService;
        #endregion

        public ImageServer(ILoggingService loggingService, IImageServiceModel imageServiceModel)
        {
            this.imageController = new ImageController(imageServiceModel);
            this.loggingService = loggingService;
        }

        public void AddDirectoryHandler(string path) // path to directory
        {
            string[] extensions = { "*.jpg", "*.png", "*.gif", "*.bmp" };
           IDirectoryHandler directoryHandler = new DirectoyHandler()
        }

    }
}
