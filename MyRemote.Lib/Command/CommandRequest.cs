using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRemote.Lib.Command
{
    [Serializable]
    public class CommandRequest
    {
        public string Id { get; set; }
        public string ActionId { get; set; }
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();

        public string this[string key]
        {
            get
            {
                return Parameters[key];
            }
            set
            {
                Parameters[key] = value;
            }
        }
    }
}
