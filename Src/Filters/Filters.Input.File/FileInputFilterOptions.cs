using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect.Filters.Input.File
{
    internal class FileInputFilterOptions
    {
        public string[] Paths { get; set; }

        public FileInputOptionsMulitlineModel Mulitline { get; set; }
    }

    internal class FileInputOptionsMulitlineModel
    {
        public string Pattern { get; set; }

        public string What { get; set; }
    }
}
