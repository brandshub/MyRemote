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
    //[XamlCompilation(XamlCompilationOptions.)]
    public partial class KeyboardView : ContentPage
    {
        public KeyboardForm KeyboardForm { get; set; }

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

        }
    }
}