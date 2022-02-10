using MyRemote.Lib.Command;
using MyRemote.Lib.Menu.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyRemote.Lib.Configuration
{
    public static class ConfigLoader
    {
        public static Config LoadConfig()
        {
            var config = new Config();

            config.Forms = new List<Form>();

            var parentDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config");
            if (!Directory.Exists(parentDir))
                throw new DirectoryNotFoundException("Configuration folder is missing!");


            var serverPath = Path.Combine(parentDir, "server.json");
            if (File.Exists(serverPath))
            {
                var content = File.ReadAllText(serverPath);
                config.Server = JsonConvert.DeserializeObject<Server.Server>(content, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }
            else
            {
                throw new FileNotFoundException("Required server.json is missing!");
            }
            

            var keyBoardsDir = Path.Combine(parentDir, "keyboards");
            if (Directory.Exists(keyBoardsDir))
            {
                var files = Directory.GetFiles(keyBoardsDir, "*.json");
                foreach (var filePath in files)
                {
                    var content = File.ReadAllText(filePath);
                    var keyboard = JsonConvert.DeserializeObject<ConfigModel<KeyboardForm>>(content, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    });

                    if (keyboard.Enabled)
                        config.Forms.Add(keyboard.Item);
                }
            }

            var formsDir = Path.Combine(parentDir, "forms");
            if (Directory.Exists(formsDir))
            {
                var files = Directory.GetFiles(formsDir, "*.json");
                foreach (var filePath in files)
                {
                    var content = File.ReadAllText(filePath);
                    var form = JsonConvert.DeserializeObject<ConfigModel<Form>>(content, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    });

                    if (form.Enabled)
                        config.Forms.Add(form.Item);
                }
            }

            var actionsPath = Path.Combine(parentDir, "actions.json");
            if (File.Exists(actionsPath))
            {
                var content = File.ReadAllText(actionsPath);
                config.CommandActions = JsonConvert.DeserializeObject<List<Action.CommandAction>>(content, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }


            var requestsPath = Path.Combine(parentDir, "requests.json");
            if (File.Exists(requestsPath))
            {
                var content = File.ReadAllText(requestsPath);
                config.Requests = JsonConvert.DeserializeObject<List<CommandRequest>>(content, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }

            return config;
        }
    }
}
