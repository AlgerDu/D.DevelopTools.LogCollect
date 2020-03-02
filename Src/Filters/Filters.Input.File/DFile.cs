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

        public string FullPath => _info.FullName;

        public string Name => _info.Name;

        public override string ToString()
        {
            return $"File[{_info.FullName}]";
        }
    }
}
