using MyRemote.AndroidClient.ViewModels;
using MyRemote.Lib.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using MyRemote.AndroidClient.Business;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyRemote.Lib.Action;

namespace MyRemote.AndroidClient.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FileListView : ContentPage
    {
        public ObservableCollection<FileEntryViewModel> Items { get; set; }

        public event Action<FileEntry> EntryClicked;

        public FileListView(string path, FileEntry[] files)
        {
            InitializeComponent();

            PathZ.Text = path;
            Items = new ObservableCollection<FileEntryViewModel>(files.Select(f => new FileEntryViewModel(f)));
            
            FilesList.ItemsSource = Items;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            // await DisplayAlert("Item Tapped", "An item was tapped.", "OK");
            /*
            var commandAction = Globals.CurrentConfig.CommandActions.FirstOrDefault(a => a.Id == ActionId);

            var request = RunProcessAction.GenericRequest()


            Lib.Server.Server.SendRequest(Globals.CurrentConfig, commandAction.RequestThis());
            */
            if (EntryClicked != null)
            {
                EntryClicked((e.Item as FileEntryViewModel).File);
            }

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
