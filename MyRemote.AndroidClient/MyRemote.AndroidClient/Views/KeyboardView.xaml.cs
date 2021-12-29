using MyRemote.AndroidClient.Business;
using MyRemote.AndroidClient.ViewModels;
using MyRemote.Lib.Action;
using MyRemote.Lib.Menu.Forms;
using MyRemote.Lib.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyRemote.AndroidClient.Views
{
    //[XamlCompilation(XamlCompilationOptions.)]
    public partial class KeyboardView : ContentPage
    {
        public KeyboardForm KeyboardForm { get; set; }

        // public List<KeyViewModel> Items { get; set; }

        public KeyboardView()
        {
            InitializeComponent();
        }

        public KeyboardView(KeyboardForm keyboardForm)
        {
            KeyboardForm = keyboardForm;
            InitializeComponent();
            BuildForm();
        }


        private void BuildForm()
        {
            int max = KeyboardForm.Buttons.Select(r => r.Sum(p => p.Width)).Max();

            Grid grid = new Grid();

            var rows = new RowDefinitionCollection();

            for (int i = 0; i < KeyboardForm.Buttons.Length; i++)
                rows.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var cols = new ColumnDefinitionCollection();

            for (int i = 0; i < max; i++)
                cols.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            grid.RowDefinitions = rows;
            grid.ColumnDefinitions = cols;

            for (int i = 0; i < grid.RowDefinitions.Count; i++)
            {
                var arr = KeyboardForm.Buttons[i];
                int left = 0;

                for (int j = 0; j < arr.Length; j++)
                {
                    var btn = new Xamarin.Forms.Button();
                    var model = new KeyViewModel(arr[j]); ;

                    btn.Text = model.Text;
                    btn.BackgroundColor = model.Color;
                    btn.HorizontalOptions = LayoutOptions.Fill;

                    btn.BindingContext = model;
                    btn.Clicked += Btn_Clicked;

                    grid.Children.Add(btn, left, i);

                    if (model.Width > 1)
                    {
                        Grid.SetColumnSpan(btn, model.Width);
                    }

                    left += model.Width;

                }
            }
            Content = grid;
        }


        private async void Btn_Clicked(object sender, EventArgs e)
        {
            var btn = (Xamarin.Forms.Button)sender;
            var key = (KeyViewModel)btn.BindingContext;

            await Task.Run(() =>
            {
                Server.SendRequest(Globals.CurrentConfig, KeyboardAction.KeyStrokeRequest(key.Keystroke));
            });
        }
    }
}