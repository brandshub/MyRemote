using MyRemote.AndroidClient.Business;
using MyRemote.AndroidClient.ViewModels;
using MyRemote.Lib.Action;
using MyRemote.Lib.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyRemote.AndroidClient.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WindowsListView : ContentPage
    {
        public ObservableCollection<WindowModel> Items { get; set; }

        public WindowsListView(Dictionary<string, string> windows)
        {
            InitializeComponent();
            MyListView.ItemsSource = new ObservableCollection<WindowModel>(windows.ToList().Select(p => new WindowModel { Caption = p.Value, hwnd = p.Key }));
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var arg = e.Item as WindowModel;
            await EntryClicked(arg);

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private async Task EntryClicked(WindowModel window)
        {
            string hwnd = window.hwnd;

            var request = FocusWindowAction.RequestThis(hwnd);
            await Server.SendRequestAsync(Globals.CurrentConfig, request);


        }
    }
}
