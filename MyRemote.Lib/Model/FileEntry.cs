using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRemote.Lib.Model
{
    public class FileEntry
    {
        [JsonProperty("f")]
        public string FileName { get; set; }
        [JsonProperty("d")]
        public bool IsDirectory { get; set; }
        [JsonProperty("dt")]
        public DateTime DatetTimeChanged { get; set; }
    }
}
