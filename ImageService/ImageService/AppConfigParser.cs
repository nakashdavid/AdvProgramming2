using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ImageService
{
    class AppConfigParser
    {
        public string[] Handler { get; }
        public string SourceName { get; }
        public string LogName { get; }
        public string OutputDirectory { get; }
        public int ThumbnailSize { get; }

        /// <summary>
        /// This class is in charge of parsing the given App.config file using ConfigurationManager.
        /// </summary>
        public AppConfigParser()
        {
            this.Handler = ConfigurationManager.AppSettings["Handler"].Split(';');
            this.OutputDirectory = ConfigurationManager.AppSettings["OutputDir"];
            this.SourceName = ConfigurationManager.AppSettings["SourceName"];
            this.LogName = ConfigurationManager.AppSettings["LogName"];
            this.ThumbnailSize = int.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
        }
    }
}
