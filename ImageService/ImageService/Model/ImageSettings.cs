using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageService.Model
{
    class ImageSettings
    {
        // init regex
        private static Regex regex = new Regex(":");

        /// <summary>
        /// 
        /// get image creation date and time
        /// </summary>
        /// <param name="imgPath"></param>
        /// <returns>DateTime format output</returns>
        public static DateTime GetImageDate(string imgPath)
        {
            try
            {
                // filestream
                using (FileStream fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read))
                // get image from stream
                using (Image myImage = Image.FromStream(fs, false, false))
                {
                    // init image properties
                    PropertyItem propItem = myImage.GetPropertyItem(36867);
                    // get date taken
                    string dateTaken = regex.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                    // parse date to correct format
                    return DateTime.Parse(dateTaken);
                }
            }
            catch (Exception)
            {
                return File.GetCreationTime(imgPath);
            }
        }
    }
}
