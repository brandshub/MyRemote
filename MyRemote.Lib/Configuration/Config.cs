using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MyRemote.Lib.Action;
using MyRemote.Lib.Command;
using MyRemote.Lib.Menu;
using MyRemote.Lib.Menu.Forms;
using Newtonsoft.Json;

namespace MyRemote.Lib.Configuration
{
    public class Config
    {
        public Server.Server Server { get; set; }

        public List<Menu.Menu> Menus { get; set; }

        public List<Form> Forms { get; set; }

        public List<CommandRequest> Requests { get; set; }

        public List<CommandAction> CommandActions { get; set; }

        public bool ShouldSerializeServer()
        {            
            return false;
        }


        public static Config LoadFromFile()
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.json");
            if (!File.Exists(path))
                throw new FileNotFoundException("Unable to load config!");

            var json = File.ReadAllText(path);

            return FromString(json);
        }

        public static Config FromString(string json)
        {
            var config = JsonConvert.DeserializeObject<Config>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return config;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }
    }
}
