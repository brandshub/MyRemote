using MyRemote.AndroidClient.Business;
using MyRemote.Lib.Server;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyRemote.AndroidClient
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new NavigationPage(new MainPage());


            MainPage = new AppShell();
        }

        protected async override void OnStart()
        {
           
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
