using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Model
{
    public class ImageServiceModel : IImageServiceModel
    {
        #region Members
        private string outputDirectory;
        private int thumbnailSize;
        #endregion

        public ImageServiceModel(string output, int tSize)
        {
            this.outputDirectory = output;
            this.thumbnailSize = tSize;
        }
        public string AddFile(string[] args, out bool result)
        {
            string newFilePath = " ";
            try
            {
                if (args.Length != 2)
                {
                    result = false;
                    return "Error: Not enough parameters - file name " + args[0] +" or path " + args[1] +" are incorrect";
                }

                string filePath = args[0];
                string fileName = args[1];
                if (!File.Exists(filePath))
                {
                    result = false;
                    return "Error: the path " + args[0] + " does not exist!";
                }
                if (!Directory.Exists(this.outputDirectory))
                {
                    // create directory
                    DirectoryInfo directoryInfo = Directory.CreateDirectory(this.outputDirectory);
                    // make directory hidden
                    directoryInfo.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }

                string year = 
            } catch (Exception e)
            {
                result = false;
                return "Path file exception: " + newFilePath + ": " + e.ToString();
            }
        }
    }
}
