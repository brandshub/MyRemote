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
using MyRemote.Lib.Server;
using Newtonsoft.Json;
using System.IO;

namespace MyRemote.AndroidClient.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FileListView : ContentPage
    {
        public ObservableCollection<FileEntryViewModel> Items { get; set; }

        public event Action<FileEntry> EntryClicked;

        public string FilePath { get; private set; }

        public FileListView(string path, FileEntry[] files)
        {
            InitializeComponent();
            InitFileData(path, files);
        }


        private void InitFileData(string path, FileEntry[] files)
        {
            FilePath = path;

            var parentDir = new FileEntry { IsDirectory = true, DatetTimeChanged = DateTime.MinValue, FileName = ".." };

            var source = new System.Collections.Generic.List<FileEntryViewModel>(files.Length + 1);
            source.Add(new FileEntryViewModel(parentDir));
            source.AddRange(files.Select(f => new FileEntryViewModel(f)));

            Items = new ObservableCollection<FileEntryViewModel>(source);

            FilesList.ItemsSource = Items;
        }


        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            defaultActivityIndicator.IsRunning = true;
            // await DisplayAlert("Item Tapped", "An item was tapped.", "OK");
            /*
            var commandAction = Globals.CurrentConfig.CommandActions.FirstOrDefault(a => a.Id == ActionId);

            var request = RunProcessAction.GenericRequest()


            Lib.Server.Server.SendRequest(Globals.CurrentConfig, commandAction.RequestThis());
            */

            var file = e.Item as FileEntryViewModel;

            if (EntryClicked != null)
            {
                EntryClicked(file.File);
            }

            IsBusy = true;

            await FileEntryClicked(file);

            defaultActivityIndicator.IsRunning = false;
            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private async Task FileEntryClicked(FileEntryViewModel file)
        {
            string rawName = file.File.FileName;

            if (file.File.IsDirectory)
            {
                string dir = System.IO.Path.Combine(FilePath, file.File.FileName);

                var request = ListFilesAction.ListDirectory(dir);

                var resp = await Server.SendRequestAsync(Globals.CurrentConfig, request);

                if (resp.StatusCode == 0)
                {
                    var filesJson = resp[ListFilesAction.OP_FILELIST];
                    var files = JsonConvert.DeserializeObject<FileEntry[]>(filesJson);

                    var path = resp[ListFilesAction.IP_PATH];

                    InitFileData(path, files);
                }
            }
            else
            {
                var fullPath = Path.Combine(FilePath, rawName);

                var request = RunProcessAction.GenericRequest(fullPath);
                await Server.SendRequestAsync(Globals.CurrentConfig, request);

                await Application.Current.MainPage.Navigation.PopAsync();
            }

        }
    }
}
