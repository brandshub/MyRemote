using MyRemote.Lib.Menu.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyRemote.AndroidClient.ViewModels
{
    public class KeyViewModel : ViewModelBase
    {
        private Lib.Menu.Forms.Button _button;
        private Color _color;


        public KeyViewModel(Lib.Menu.Forms.Button button)
        {
            _button = button;

            if (string.IsNullOrEmpty(_button.ColorHex))
                _color = Color.LightGray;
            else
                _color = Color.FromHex(_button.ColorHex);

            Keystroke = button.Key;

            ActionId = button.RequestId;
        }


        public string Text => _button.Text;

        public int Width => _button.Width;

        public Color Color => _color;

        public string ActionId { get; private set; }

        public string Keystroke { get; private set; }

    }
}
