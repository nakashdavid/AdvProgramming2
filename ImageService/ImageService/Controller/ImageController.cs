using ImageService.Commands;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        private IImageServiceModel imageServiceModel;                      // The Model Object
        private Dictionary<int, ICommand> commands;

        private class TaskResult
        {
            public TaskResult(string result, bool boolean)
            {
                this.result = result;
                this.boolean = boolean;
            }
            public string result { get; set; }
            public bool boolean { get; set; }
        }

        public ImageController(IImageServiceModel Model)
        {
            this.imageServiceModel = Model;                    // Storing the Model Of The System
            commands = new Dictionary<int, ICommand>()
            {
                { (int) CommandCategoryEnum.ADD_FILE, new NewFileCommand(this.imageServiceModel) }
                // For Now will contain NEW_FILE_COMMAND
            };
        }
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {

            // Write Code Here
            if (commands.ContainsKey(commandID))
            {
                Task<TaskResult> task = new Task<TaskResult>(() =>
                {
                    bool boolean;
                    ICommand command = commands[commandID];
                    string result = command.Execute(args, out boolean);
                    return new TaskResult(result, boolean);
                });

                task.Start();
                TaskResult taskRes = task.Result;
                resultSuccesful = task.Result.boolean;
                return taskRes.result;

            } else {

                resultSuccesful = false;
                return "Command doesn't exist!";

            }
        }
    }
}
