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
    public class DirectoyHandler : IDirectoryHandler
    {

        #region Members
        private string directoryPath;
        private IImageController imageController;
        private ILoggingService loggingService;
        private string[] extensions;
        private List<FileSystemWatcher> directoryWatchers;

        #endregion
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed


        public DirectoyHandler(string path, IImageController imageController, ILoggingService loggingService, string []extensions)
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

            // Run through all the files and send to "add file"
        }

        public void CloseFileWatcher(object source, DirectoryCloseEventArgs args)
        {
            if (args.DirectoryPath.Equals(this.directoryPath) || args.DirectoryPath.Equals("*"))
            {
                // set watchers to not be able to send events anymore
                foreach (FileSystemWatcher fileSystemWatcher in this.directoryWatchers)
                {
                    fileSystemWatcher.EnableRaisingEvents = false;
                }
                this.loggingService.Log(this, );

            }
        }
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void StartHandleDirectory(string dirPath)
        {
            throw new NotImplementedException();
        }

        // Implement Here!
    }
}
