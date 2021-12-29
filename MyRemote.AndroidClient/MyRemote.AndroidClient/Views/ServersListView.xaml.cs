using MyRemote.AndroidClient.Business;
using MyRemote.AndroidClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyRemote.AndroidClient.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServersListView : ContentPage
    {
        public ObservableCollection<ServerCredentialsViewModel> Items { get; set; }


        public ServersListView()
        {
            InitializeComponent();
        }


        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ServerView());
        }


        protected override void OnAppearing()
        {
            /*


              DeleteCommand = new Command(async (p) =>
              {

                  var item = p as IncrementableViewModel;
                  if (item != null)
                  {
                      string result = await DisplayActionSheet("Do you really want to delete " + item.Title, "Cancel", "Delete");
                      if (result == "Delete")
                      {
                          await App.Incrementables.Remove(item.Incrementable);
                          Items.Remove(item);
                      }
                  }
              });

              DateRange = Preferences.Get("dateRange", "All Time");*/

            Items = new ObservableCollection<ServerCredentialsViewModel>(Globals.SavedServers.Select(p => new ServerCredentialsViewModel(p)));
            MyListView.ItemsSource = Items;
        }

        private void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var selected = e.Item as ServerCredentialsViewModel;
            var flag = selected.Selected;

            selected.Selected = !flag;
            if (selected.Selected)
            {
                foreach (var item in Items)
                    if (item != selected && item.Selected)
                        item.Selected = false;
            }


            ((ListView)sender).SelectedItem = null;
        }
    }
}