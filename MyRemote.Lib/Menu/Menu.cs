using MyRemote.Lib.Menu.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyRemote.Lib.Menu
{
    public class Menu
    {
        public string Title { get; set; }

        public List<string> FormIds { get; set; }

        [JsonIgnore]
        public List<Form> Forms { get; set; }
    }
}
