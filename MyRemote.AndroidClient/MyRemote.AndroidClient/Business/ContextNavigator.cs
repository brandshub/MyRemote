using MyRemote.AndroidClient.Views;
using MyRemote.Lib.Action;
using MyRemote.Lib.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyRemote.AndroidClient.Business
{
    public class ContextNavigator
    {

        public static async Task TryPerformContextedNavigation(CommandRequest request, CommandResponse response)
        {
            if (request.ActionId == ListActiveWindowsAction.CODE)
            {
                var view = new WindowsListView(response.Payload);
                await Application.Current.MainPage.Navigation.PushAsync(view);
            }
        }
    }
}
