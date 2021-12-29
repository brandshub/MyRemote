using MyRemote.Lib.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyRemote.AndroidClient.ViewModels
{
    public class FileEntryViewModel : ViewModelBase
    {
        public FileEntry File { get; set; }

        public FileEntryViewModel(FileEntry file)
        {
            File = file;
        }

        public string Type
        {
            get
            {
                return File.IsDirectory ? "DIR" : "FILE";
            }
        }

        public string Name => File.FileName;
        public string DateTimeChanged => File.DatetTimeChanged.ToString("yyyy-MM-dd HH:mm");

    }
}
