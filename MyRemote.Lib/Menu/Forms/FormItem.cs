using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRemote.Lib.Menu.Forms
{
    public class FormItem
    {
        public string Id { get; set; }

        [JsonProperty("t")]
        public string Text { get; set; }
        public string ColorHex { get; set; }

        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }

        [JsonProperty("w")]
        public int Width { get; set; }
        [JsonProperty("h")]
        public int Height { get; set; }

        public string RequestId { get; set; }

    }
}
