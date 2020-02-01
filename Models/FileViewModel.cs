using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTH_Upload.Models
{
    public class FileViewModel
    {

        public IDirectoryContents PhysicalFiles { get; set; }

        public int FileCount { get; set; }

        public string CurrentDirectory { get; set; }

        //public FileViewModel() { CurrentDirectory = string.Empty; }

        //public FileViewModel(string t) { this.CurrentDirectory = t; }
    }
}
