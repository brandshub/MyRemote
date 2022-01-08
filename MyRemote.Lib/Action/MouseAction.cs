using MyRemote.Lib.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;

namespace MyRemote.Lib.Action
{
    public class MouseAction : CommandAction
    {
        public const string CODE = "MOUSE";
        public const string COMMAND = "COMMAND";
        public const string SCROLL = "SCROLL";
        public const string LCLICK = "LCLICK";
        public const string RCLICK = "RCLICK";

        public const string MOVE = "MOVE";

        public const string ARG1 = "ARG1";
        public const string ARG2 = "ARG2";

        public MouseAction(Dictionary<string, string> parameters) : base(parameters)
        {
        }

        public override string Code => CODE;


        public override CommandResponse Execute()
        {
            if (this[COMMAND] != SCROLL)
                throw new NotImplementedException();

            var x = new InputSimulator();

            string deltaStr = this[ARG1];

            if (string.IsNullOrEmpty(deltaStr) || !int.TryParse(deltaStr, out int delta))
            {
                x.Mouse.VerticalScroll(1);
                return new CommandResponse();
            }


            x.Mouse.VerticalScroll(delta);
            return new CommandResponse();
        }

        public static CommandRequest Scroll(int delta)
        {
            return new CommandRequest
            {
                Id = "GENERIC_AUTO_" + CODE,
                ActionId = CODE,
                Parameters = new Dictionary<string, string> { { COMMAND, SCROLL }, { ARG1, delta.ToString() } }
            };
        }
    }
}
