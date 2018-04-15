using ImageService.Infrastructure;
using ImageService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class NewFileCommand : ICommand
    {
        private IImageServiceModel imageServiceModel;

        public NewFileCommand(IImageServiceModel Model)
        {
            this.imageServiceModel = Model;            // Storing the Model
        }

        public string Execute(string[] args, out bool result)
        {
            // The String Will Return the New Path if result = true, and will return the error message

            return this.imageServiceModel.AddFile(args, out result);
        }
    }
}
