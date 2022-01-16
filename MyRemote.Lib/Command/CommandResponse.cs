using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRemote.Lib.Command
{
    [Serializable]
    public class CommandResponse
    {
        public int StatusCode { get; set; } = 0;

        public Dictionary<string, string> Payload { get; set; }

        public CommandResponse(Dictionary<string, string> payload)
        {
            Payload = payload;
        }

        public string this[string key]
        {
            get
            {
                return Payload[key];
            }
        }


        public CommandResponse() { Payload = new Dictionary<string, string>(); }

    }
}
