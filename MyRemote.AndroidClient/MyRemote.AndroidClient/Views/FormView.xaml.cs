using MyRemote.AndroidClient.Business;
using MyRemote.Lib.Command;
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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormView : ContentPage
    {
        public Form Form { get; private set; }

        public FormView()
        {
            InitializeComponent();
        }

        public FormView(Form form)
        {
            InitializeComponent();
            Form = form;
            Title = Form.Title;

            BuildForm();
        }


        private void BuildForm()
        {

            Grid grid = new Grid();

            var rows = new RowDefinitionCollection();

            for (int i = 0; i < Form.Controls.Count; i++)
                rows.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var cols = new ColumnDefinitionCollection();

            for (int i = 0; i < 1; i++)
                cols.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            grid.RowDefinitions = rows;
            grid.ColumnDefinitions = cols;

            for (int i = 0; i < Form.Controls.Count; i++)
            {
                var ctrl = Form.Controls[i];
                var btn = new Xamarin.Forms.Button();

                btn.Text = ctrl.Text;
                //btn.BackgroundColor = ctrl.Color;
                btn.HorizontalOptions = LayoutOptions.Fill;

                btn.BindingContext = ctrl;
                btn.Clicked += Btn_Clicked;

                grid.Children.Add(btn, 0, i);

            }

            Content = grid;
        }


        private async void Btn_Clicked(object sender, EventArgs e)
        {
            var btn = (Xamarin.Forms.Button)sender;
            var formItem = (FormItem)btn.BindingContext;

            var command = Globals.FindActionById(formItem.RequestId);

            await Server.SendRequestAsync(Globals.CurrentConfig, command.RequestThis());

        }
    }
}