using MyRemote.Lib.Command;
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
            var fis = di.GetFiles("*", SearchOption.TopDirectoryOnly);
            var dis = di.GetDirectories("*", SearchOption.TopDirectoryOnly);

            var data = new Dictionary<string, string>();

            var entries = fis.Select(p => new FileEntry
            {
                FileName = p.Name,
                IsDirectory = p.Attributes.HasFlag(FileAttributes.Directory),
                DatetTimeChanged = p.LastWriteTime
            }).ToList();

            entries.AddRange(dis.Select(p => new FileEntry
            {
                FileName = p.Name,
                IsDirectory = true,
                DatetTimeChanged = p.LastWriteTime
            }));

            entries.Sort((f1, f2) => { if (f1.IsDirectory == f2.IsDirectory) return string.Compare(f1.FileName, f2.FileName, true); if (f1.IsDirectory) return -1; return 1; });

            data[OP_FILELIST] = JsonConvert.SerializeObject(entries);
            data[IP_PATH] = di.FullName;

            return new CommandResponse(data);
        }

        public static CommandRequest ListDirectory(string directory)
        {
            return new CommandRequest
            {
                ActionId = CODE,
                Parameters = new Dictionary<string, string> { { IP_PATH, directory } }
            };
        }
    }
}
