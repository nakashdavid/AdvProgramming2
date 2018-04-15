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

        private class TaskRes
        {
            /// <summary>
            /// Custom class to save the task's result and success parameter.
            /// </summary>
            /// <param name="result"></param>
            /// <param name="boolean"></param>
            public TaskRes(string result, bool boolean)
            {
                this.result = result;
                this.boolean = boolean;
            }
            public string result { get; set; }
            public bool boolean { get; set; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Model"></param>
        public ImageController(IImageServiceModel Model)
        {
            this.imageServiceModel = Model;                    // Storing the Model Of The System
            commands = new Dictionary<int, ICommand>()
            {
                { (int) CommandCategoryEnum.AddFile, new NewFileCommand(this.imageServiceModel) }
                // For Now will contain NEW_FILE_COMMAND
            };
        }

        /// <summary>
        /// Execute the command given by the directory handler, outputs error if command doesn't exist.
        /// </summary>
        /// <param name="commandID"></param>
        /// <param name="args"></param>
        /// <param name="resultSuccesful"></param>
        /// <returns></returns>
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {

            // Write Code Here
            if (commands.ContainsKey(commandID))
            {
                Task<TaskRes> task = new Task<TaskRes>(() =>
                {
                    bool boolean;
                    ICommand command = commands[commandID];
                    string result = command.Execute(args, out boolean);
                    return new TaskRes(result, boolean);
                });

                task.Start();
                TaskRes taskRes = task.Result;
                resultSuccesful = task.Result.boolean;
                return taskRes.result;

            } else {

                resultSuccesful = false;
                return "Command doesn't exist!";

            }
        }
    }
}
