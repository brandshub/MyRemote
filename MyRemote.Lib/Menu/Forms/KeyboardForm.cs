using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRemote.Lib.Menu.Forms
{
    public class KeyboardForm: Form
    {
        public Button[][] Buttons { get; set; }
        

        public (int width, int height) CalculateGrid()
        {

            return (5, 10);
        }
    }
}
