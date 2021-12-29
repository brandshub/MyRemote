﻿using MyRemote.Lib.Command;
using MyRemote.Lib.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyRemote.Lib.Action
{
    public class ListFilesAction : CommandAction
    {
        public const string CODE = "LIST_FILES";
        public const string IP_PATH = "path";
        public const string OP_FILELIST = "fileList";
        public override string Code => CODE;

        public ListFilesAction(Dictionary<string, string> parameters) : base(parameters)
        {
        }

        public override CommandResponse Execute()
        {
            var path = this[IP_PATH];

            var di = new DirectoryInfo(path);
            var fi = di.GetFiles("*", SearchOption.TopDirectoryOnly);

            var data = new Dictionary<string, string>();

            var entries = fi.Select(p => new FileEntry
            {                
                FileName = p.Name,
                IsDirectory = p.Attributes.HasFlag(FileAttributes.Directory),
                DatetTimeChanged = p.LastWriteTime
            }).ToArray();

            data[OP_FILELIST] = JsonConvert.SerializeObject(entries);

            return new CommandResponse(data);
        }
    }
}