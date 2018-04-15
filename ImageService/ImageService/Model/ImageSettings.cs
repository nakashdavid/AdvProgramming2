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
        private static Regex regex = new Regex(":");
        public static DateTime GetImageDate(string imgPath)
        {
            try
            {
                using (FileStream fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read))
                using (Image myImage = Image.FromStream(fs, false, false))
                {
                    PropertyItem propItem = myImage.GetPropertyItem(36867);
                    string dateTaken = regex.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
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
