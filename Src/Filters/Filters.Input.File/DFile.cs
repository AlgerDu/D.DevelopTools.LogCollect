using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace D.DevelopTools.LogCollect.Filters.Input.File
{
    internal class DFile : IDFile
    {
        FileInfo _info;

        public DFile(FileInfo info)
        {
            _info = info;
        }

        public override string ToString()
        {
            return $"File[{_info.FullName}]";
        }
    }
}
