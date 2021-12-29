using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRemote.Lib.Menu.Forms
{
    public class Form
    {
        public string Id { get; set; }

        public string Title { get; set; }
        public List<FormItem> Controls { get; set; }

    }
}
