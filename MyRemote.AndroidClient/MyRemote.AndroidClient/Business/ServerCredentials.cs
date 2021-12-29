using System;
using System.Collections.Generic;
using System.Text;

namespace MyRemote.AndroidClient.Business
{
    public class ServerCredentials
    {
        public string Title { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public string Secret { get; set; }        
        public bool Selected { get; set; }
    }
}
