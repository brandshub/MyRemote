using MyRemote.AndroidClient.Business;
using MyRemote.Lib.Menu.Forms;
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
    public partial class MenuListView : ContentPage
    {
        public List<Form> Items { get; private set; }

        public MenuListView()
        {
            InitializeComponent();
        }

        public MenuListView(List<Form> forms)
        {
            InitializeComponent();
            Items = forms;

            MyListView.ItemsSource = Items;
            Title = "Forms";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Items = Globals.CurrentConfig.Forms;
            MyListView.ItemsSource = Items;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var item = (Xamarin.Forms.Button)sender;

            var context = item.BindingContext as Form;

            if (context.GetType() == typeof(KeyboardForm))
                await Application.Current.MainPage.Navigation.PushAsync(new KeyboardView(context as KeyboardForm));
            else
                await Application.Current.MainPage.Navigation.PushAsync(new FormView(context));
        }
    }
}