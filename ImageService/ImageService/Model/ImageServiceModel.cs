using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
                Thread.Sleep(500);

                if (File.Exists(filePath))
                {
                    if (!Directory.Exists(this.outputDirectory))
                    {
                        // create directory
                        DirectoryInfo directoryInfo = Directory.CreateDirectory(this.outputDirectory);
                        // make directory hidden
                        directoryInfo.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                        
                    }

                    Thread.Sleep(500);
                    return MoveFile(filePath, fileName, outputDirectory, out result);
                    
                } else
                {
                    result = false;
                    return "Error: the path " + args[0] + " does not exist!";
                }
                
                
            } catch (Exception e)
            {
                result = false;
                return "Path file exception: " + newFilePath + ": " + e.ToString();
            }
        }


        public string MoveFile(string filePath, string fileName, string outputFolder, out bool result)
        {
            string newFilePath = " ";

            string year = ImageSettings.GetImageDate(filePath).Year.ToString();
            string month = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(ImageSettings.GetImageDate(filePath).Month).ToString();

            // make new photo dir
            string dateOutputFolder = outputFolder + "\\" + year + "\\" + month;
            Directory.CreateDirectory(dateOutputFolder);

            // make thumbnail dir
            string thumbnailOutputFolder = outputFolder + "\\thumbnails\\" + year + "\\" + month;
            Directory.CreateDirectory(thumbnailOutputFolder);

            string underscore = "_";
            newFilePath = dateOutputFolder + "\\" + fileName;

            // if same name file already exists add '_' to it
            while (File.Exists(newFilePath))
            {
                fileName = underscore + fileName;
                newFilePath = dateOutputFolder + "\\" + fileName;
            }

            string thumbnailPath = thumbnailOutputFolder + "\\" + fileName;
            
            File.Move(filePath, newFilePath);

            Image img = Image.FromFile(newFilePath);
            Image thumbnail = img.GetThumbnailImage(thumbnailSize, thumbnailSize, () => false, IntPtr.Zero);

            thumbnail.Save(Path.ChangeExtension(thumbnailPath, "thumb"));
            
            // dispose of image and thumbnail
            img.Dispose();
            thumbnail.Dispose();

            // outputs
            result = true;
            return "Image " + fileName + " added to " + year + ", " + month + ".";
        }

    }
}
