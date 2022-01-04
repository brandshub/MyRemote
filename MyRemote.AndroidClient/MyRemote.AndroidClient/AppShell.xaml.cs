using MyRemote.AndroidClient.Views;
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

            Business.Globals.LoadServerData();
            RegisterRoutes();

         //   if (Globals.SavedServers.Count == 0)
         //       Navigation.PushAsync(new ServerView());

            //   await Shell.Current.GoToAsync("ServerView");
        }

        private void RegisterRoutes()
        {
            Routes.Add("serverslist", typeof(ServersListView));
            Routes.Add("keyb", typeof(KeyboardView));
            Routes.Add("forms", typeof(MenuListView));


            foreach (var item in Routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }

    }
}