using MyRemote.Lib.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRemote.Lib.Action
{
    public class FocusWindowAction : CommandAction
    {
        public const string CODE = "FOCUS_WINDOW";
        public const string IN_HWND = "hwnd";

        public FocusWindowAction(Dictionary<string, string> parameters) : base(parameters)
        {
        }

        public override string Code => CODE;

        public override CommandResponse Execute()
        {
            var hwnd = this[IN_HWND];
            var parsed = new IntPtr(int.Parse(hwnd));
            WinApiHelper.FocusWindow(parsed);

            return new CommandResponse();
        }

        public static CommandRequest RequestThis(string hwnd)
        {
            return new CommandRequest { ActionId = CODE, Parameters = new Dictionary<string, string> { { IN_HWND, hwnd } } };
        }
    }
}
