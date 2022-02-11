using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyRemote.Lib.Command;


namespace MyRemote.Lib.Action
{
    public class ListActiveWindowsAction : CommandAction
    {
        public const string CODE = "LIST_WINDOWS";
        public override string Code => CODE;

     

        public ListActiveWindowsAction(Dictionary<string, string> parameters) : base(parameters)
        {
        }

        public override CommandResponse Execute()
        {
            var windows = WinApiHelper.GetOpenWindows();
            return new CommandResponse(windows.ToDictionary(p => p.Key.ToString(), p => p.Value));
        }
    }
}
