using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRemote.Lib.Configuration
{
    public class ConfigModel<T>
    {
        public bool Enabled { get; set; }

        public string[] AssociatedProcesses { get; set; }

        public T Item { get; set; }

    }
}
