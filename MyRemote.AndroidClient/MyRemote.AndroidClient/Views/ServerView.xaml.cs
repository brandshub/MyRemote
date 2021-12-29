using MyRemote.AndroidClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyRemote.AndroidClient.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServerView : ContentPage
    {
        public ServerCredentialsViewModel Item { get; set; }
        public ServerView(): this(new ServerCredentialsViewModel())
        {         
        }

        public ServerView(ServerCredentialsViewModel item)
        {
            InitializeComponent();
            Item = item;
            BindingContext = Item;            
        }
    }
}