using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure.Enums
{
    public enum CommandEnum : int
    {
        NewFileCommand,
        CloseCommand
    }

    /// <summary>
    /// enum for future uses (when there are more commands required...)
    /// </summary>
    public enum CommandCategoryEnum : int
    {
        AddFile
    }
}
