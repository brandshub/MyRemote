using MyRemote.Lib.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRemote.Lib.Action
{
    public class RunProcessAction : CommandAction
    {
        public const string COMMAND = "command";
        public const string CODE = "RUN";


        public RunProcessAction(Dictionary<string, string> parameters) : base(parameters)
        {
        }

        public override string Code => CODE;

        public override CommandResponse Execute()
        {
            Process.Start(this[COMMAND]);

            return new CommandResponse();
        }

        public static CommandRequest GenericRequest(string command)
        {
            return new CommandRequest
            {
                Id = "GENERIC_AUTO_"+CODE,
                ActionId = CODE,
                Parameters = new Dictionary<string, string> { { COMMAND, command } }
            };
        }
    }
}

