using MyRemote.AndroidClient.Business;
using MyRemote.AndroidClient.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;
using Xamarin.Forms;

namespace MyRemote.AndroidClient.ViewModels
{
    public class ServerCredentialsViewModel : ViewModelBase
    {
        public ServerCredentials Server { get; set; }
        public ServerCredentialsViewModel() : this(new ServerCredentials()) { }

        public ServerCredentialsViewModel(ServerCredentials server)
        {
            Server = server;

            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            EditCommand = new Command(OnEdit);


            this.PropertyChanged +=
            (sender, e) =>
            {
                if (e.PropertyName == nameof(Title) || e.PropertyName == nameof(Port) || e.PropertyName == nameof(IpAddress))
                    SaveCommand.ChangeCanExecute();
            };
        }

        public string IpAddress
        {
            get { return Server.IpAddress; }
            set
            {                
                if (Server.IpAddress != value)
                {
                    Server.IpAddress = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Port
        {
            get { return Server.Port; }
            set
            {
                if (Server.Port != value)
                {
                    Server.Port = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Display
        {
            get
            {
                return $"{IpAddress}:{Port}";
            }
        }

        public FontAttributes SelectedFontAttr
        {
            get
            {
                if (Selected)
                    return FontAttributes.Bold;
                return FontAttributes.None;
            }
        }

        public string Secret
        {
            get { return Server.Secret; }
            set
            {

                if (Server.Secret != value)
                {
                    Server.Secret = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Title
        {
            get { return Server.Title; }
            set
            {
                if (Server.Title != value)
                {
                    Server.Title = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool Selected
        {
            get { return Server.Selected; }
            set
            {
                if (Server.Selected != value)
                {
                    Server.Selected = value;
                    OnPropertyChanged();
                    OnPropertyChanged("SelectedFontAttr");
                }
            }
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command EditCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            //await Shell.Current.GoToAsync("..");
            await Application.Current.MainPage.Navigation.PopAsync();
        }




        private async void OnSave()
        {
            if (!Globals.SavedServers.Contains(Server))
            {
                Globals.SavedServers.Add(Server);
            }
            Globals.SaveServerData();
            await Shell.Current.GoToAsync("serverslist");            
        }

        private async void OnEdit()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ServerView(this));

            // This will pop the current page off the navigation stack
            //await Shell.Current.GoToAsync("..");
        }


        private bool ValidateSave()
        {
            if (!string.IsNullOrEmpty(Server.Title) && IPAddress.TryParse(Server.IpAddress, out IPAddress temp)/* && int.TryParse(Server.Port, out int temp2)*/)
                return true;

            return false;
        }


        public bool IsValid
        {
            get
            {
                return ValidateSave();
            }
        }




    }

}
