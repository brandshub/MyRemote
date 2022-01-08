using MyRemote.AndroidClient.Business;
using MyRemote.AndroidClient.Views;
using MyRemote.Lib.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyRemote.AndroidClient
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public Dictionary<string, Type> Routes { get; private set; } = new Dictionary<string, Type>();

        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        private async void RegisterRoutes()
        {
            Routes.Add("serverslist", typeof(ServersListView));
            Routes.Add("keyb", typeof(KeyboardView));
            Routes.Add("forms", typeof(MenuListView));


            foreach (var item in Routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }

            if (Globals.CurrentConfig == null)
                await Business.Globals.TryLoadDefaultData();

            //Business.Globals.LoadServerData();
            //if (Globals.SavedServers.Count > 0)
            //{
            //    var server = Globals.GetSelectedOrFirstServer();
            //    if (server != null)
            //    {
            //        try
            //        {
            //            Globals.CurrentConfig = await Server.GetConfigAsync(server.IpAddress, server.Port, server.Secret);
            //            Globals.CurrentConfig.Server = new Server { IpAddress = server.IpAddress, Port = server.Port, Secret = server.Secret };
            //        }
            //        catch (Exception ex)
            //        {
            //            Globals.CurrentConfig = null;
            //        }

            //    }
            //}

            if (Globals.SavedServers.Count == 0)
                await Navigation.PushAsync(new ServerView());
        }

    }
}