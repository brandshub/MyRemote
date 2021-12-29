using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRemote.Lib.Model
{
    public class FileEntry
    {
        public string FileName { get; set; }
        public bool IsDirectory { get; set; }

        public DateTime DatetTimeChanged { get; set; }
    }
}
