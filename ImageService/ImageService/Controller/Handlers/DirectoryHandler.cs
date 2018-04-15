using ImageService.Model;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Model;
using System.Text.RegularExpressions;

namespace ImageService.Controller.Handlers
{
    public class DirectoryHandler : IDirectoryHandler
    {

        #region Members
        private string directoryPath;
        private IImageController imageController;
        private ILoggingService loggingService;
        private string[] extensions;
        private List<FileSystemWatcher> directoryWatchers;

        #endregion

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed


        public DirectoryHandler(string path, IImageController imageController, ILoggingService loggingService, string []extensions)
        {
            this.directoryPath = path;
            this.imageController = imageController;
            this.loggingService = loggingService;
            this.extensions = extensions;
            this.directoryWatchers = new List<FileSystemWatcher>();
        }

        public void StartHandlingDirectory()
        {
            for (int i = 0; i < extensions.Length; i++)
            {
                FileSystemWatcher directoryWatcher = new FileSystemWatcher(this.directoryPath, this.extensions[i]);
                directoryWatcher.EnableRaisingEvents = true;
                directoryWatcher.Created += new FileSystemEventHandler(FileCreated);
                this.directoryWatchers.Add(directoryWatcher);
            }
        }

        public void CloseFileWatcher(object source, DirectoryCloseEventArgs args)
        {
            if (args.DirectoryPath.Equals(this.directoryPath) || args.DirectoryPath.Equals("*"))
            {
                // set watchers' raising event status to false
                foreach (FileSystemWatcher fileSystemWatcher in this.directoryWatchers)
                {
                    fileSystemWatcher.EnableRaisingEvents = false;
                }
                this.loggingService.Log(this, new MessageRecievedEventArgs(MessageTypeEnum.INFO, "Closing directory " + this.directoryPath));
                DirectoryClose.Invoke(this, new DirectoryCloseEventArgs(this.directoryPath, "Received handler-closing message"));

            }
        }

        private void FileCreated(object sender, FileSystemEventArgs args)
        {
            // call this when the command is received
            string[] arguments = new string[] { args.FullPath, args.Name };
            int commandID = (int)CommandCategoryEnum.ADD_FILE;
            CommandRecievedEventArgs eventArgs = new CommandRecievedEventArgs(commandID, arguments, this.directoryPath);
            OnCommandRecieved(sender, eventArgs);
        }


        public void OnCommandRecieved(object sender, CommandRecievedEventArgs eventArgs)
        {
            if (eventArgs.RequestDirPath.Equals(this.directoryPath) || eventArgs.RequestDirPath.Equals("*"))
            {
                bool success;
                string result = this.imageController.ExecuteCommand(eventArgs.CommandID, eventArgs.Args, out success);
                if (success)
                {
                    this.loggingService.Log(this, new MessageRecievedEventArgs(MessageTypeEnum.INFO, result));
                }
                else
                {
                    this.loggingService.Log(this, new MessageRecievedEventArgs(MessageTypeEnum.FAIL, result));
                }
            }
        }
       
    }
}
