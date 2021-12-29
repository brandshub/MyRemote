using MyRemote.Lib.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyRemote.Lib.Action
{
    public class WaitAction : CommandAction
    {
        public WaitAction(Dictionary<string, string> parameters) : base(parameters)
        {
        }

        public override string Code { get { return "WAIT"; } }

        public override CommandResponse Execute()
        {
            Thread.Sleep(int.Parse(this["ms"]));

            return new CommandResponse();
        }   
    }
}
