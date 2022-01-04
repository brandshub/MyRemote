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
        public const string ARGS = "args";

        public const string CODE = "RUN";


        public RunProcessAction(Dictionary<string, string> parameters) : base(parameters)
        {
        }

        public override string Code => CODE;

        public override CommandResponse Execute()
        {
            string process = this[COMMAND];
            string args = this[ARGS];

            Process.Start(process, args);

            return new CommandResponse();
        }

        public static CommandRequest GenericRequest(string command, string args = null)
        {
            return new CommandRequest
            {
                Id = "GENERIC_AUTO_" + CODE,
                ActionId = CODE,
                Parameters = new Dictionary<string, string> { { COMMAND, command }, { ARGS, args } }
            };
        }
    }
}

