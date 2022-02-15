using MyRemote.AndroidClient.Views;
using MyRemote.Lib.Action;
using MyRemote.Lib.Command;
using MyRemote.Lib.Model;
using MyRemote.Lib.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyRemote.AndroidClient.Business
{
    public class ContextNavigator
    {

        public static async Task TryPerformContextedNavigation(CommandAction action, CommandRequest request, CommandResponse response)
        {
            if (request.ActionId == ListActiveWindowsAction.CODE)
            {
                var view = new WindowsListView(response.Payload);
                await Application.Current.MainPage.Navigation.PushAsync(view);
            }
            else if (action is ListFilesAction)
            {                
                var req = action.RequestThis();
                var resp = Server.SendRequest(Globals.CurrentConfig, req);

                var filesJson = resp.Payload[ListFilesAction.OP_FILELIST];
                var files = JsonConvert.DeserializeObject<FileEntry[]>(filesJson);

                var path = action[ListFilesAction.IP_PATH];

                var view = new FileListView(path, files);

                await Application.Current.MainPage.Navigation.PushAsync(view);
            }

        }
    }
}
