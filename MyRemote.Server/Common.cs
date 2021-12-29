using MyRemote.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyRemote.Server
{
    public static class Common
    {
        private static readonly object syncRoot = new object();

        private static Config _current;
        public static Config CurrentConfig
        {
            get
            {
                lock (syncRoot)
                {
                    if (_current == null)
                        _current = Config.LoadFromFile();
                    return _current;
                }
            }
        }
    }
}
