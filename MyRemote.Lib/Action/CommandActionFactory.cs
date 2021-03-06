using MyRemote.Lib.Command;
using MyRemote.Lib.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRemote.Lib.Action
{
    public class CommandActionFactory
    {

        public static CommandAction FromRequest(CommandRequest request, Config cfg)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (cfg is null)
                throw new ArgumentNullException(nameof(cfg));


            var action = cfg.CommandActions.FirstOrDefault(d => d.Id.Equals(request.ActionId));
            if (action != null)
                return action;


            if (request.ActionId == ListFilesAction.CODE)
                return new ListFilesAction(request.Parameters);

            if (request.ActionId == RunProcessAction.CODE)
                return new RunProcessAction(request.Parameters);


            if (request.ActionId == KeyboardAction.CODE)
                return new KeyboardAction(request.Parameters);

            if (request.ActionId == MouseAction.CODE)
                return new MouseAction(request.Parameters);

            if (request.ActionId == ListActiveWindowsAction.CODE)
                return new ListActiveWindowsAction(null);

            if (request.ActionId == FocusWindowAction.CODE)
                return new FocusWindowAction(request.Parameters);

            throw new Exception("Unknown action");
        }

    }
}
