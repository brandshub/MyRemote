using MyRemote.AndroidClient.Business;
using MyRemote.AndroidClient.Views;
using MyRemote.Lib;
using MyRemote.Lib.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using MyRemote.Lib.Server;
using MyRemote.Lib.Action;
using MyRemote.Lib.Model;
using MyRemote.Lib.Menu.Forms;
using MyRemote.AndroidClient.Interfaces;
using Xamarin.Forms.Internals;

namespace MyRemote.AndroidClient
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void btnClick_Clicked(object sender, EventArgs e)
        {
            var server = Globals.GetSelectedOrFirstServer();
            if (server == null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ServerView());
            }
            else
            {

                try
                {
                    Globals.CurrentConfig = await Server.GetConfigAsync(server.IpAddress, server.Port, server.Secret);
                    Globals.CurrentConfig.Server = new Server { IpAddress = server.IpAddress, Port = server.Port, Secret = server.Secret };

                    Globals.SavedServers.Where(p => p != server).ForEach(d => d.IsConnected = false);
                    server.IsConnected = true;

                    // Toast.MakeText(.MakeText(this, "Config loaded sucessfully", ToastLength.Short).Show();
                    //  await DisplayAlert("Success", "Config loaded!", "OK");

                    DependencyService.Get<IMessage>().LongAlert("Config loaded sucessfully");
                }
                catch (OperationCanceledException ex)
                {
                    await DisplayAlert("Failure", ex.CancellationToken == null ? "TCP connection failed" : "TCP connection timed out", "OK");
                }
                catch
                {
                    await DisplayAlert("Failure", "Unspecified error occured", "OK");
                }
            }
        }

        private async void btnServers_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ServersListView());
        }


        private async void btnTorrents_Clicked(object sender, EventArgs e)
        {
            var command = Globals.CurrentConfig.CommandActions.FirstOrDefault(d => d.Id == "LIST_TORRENTS");
            var req = command.RequestThis();

            var resp = Server.SendRequest(Globals.CurrentConfig, req);

            var filesJson = resp.Payload[ListFilesAction.OP_FILELIST];
            var files = JsonConvert.DeserializeObject<FileEntry[]>(filesJson);

            var path = command[ListFilesAction.IP_PATH];

            var view = new FileListView(path, files);
           /* view.EntryClicked += async (o) =>
            {
                var fullPath = Path.Combine(path, o.FileName);
                var request = RunProcessAction.GenericRequest(fullPath);
                Lib.Server.Server.SendRequest(Globals.CurrentConfig, request);

                await Application.Current.MainPage.Navigation.PopAsync();
            };
           */
            await Application.Current.MainPage.Navigation.PushAsync(view);
        }

        private async void btnKeyboard_Clicked(object sender, EventArgs e)
        {
            var form = Globals.CurrentConfig.Forms.FirstOrDefault(k => k.Id == "ytK") as KeyboardForm;
            if (form != null)
            {
                var view = new KeyboardView(form);
                await Application.Current.MainPage.Navigation.PushAsync(view);
            }
        }
    }
}
