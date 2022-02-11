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


            var keyboardsConfig = BuildList<ConfigModel<KeyboardForm>>(Path.Combine(parentDir, "keyboards"));
            config.Forms.AddRange(keyboardsConfig.Where(p => p.Enabled).Select(z => z.Item));


            var formsConfig = BuildList<ConfigModel<Form>>(Path.Combine(parentDir, "forms"));
            config.Forms.AddRange(formsConfig.Where(p => p.Enabled).Select(z => z.Item));

            config.CommandActions = BuildList<Action.CommandAction>(Path.Combine(parentDir, "actions"));
            config.Requests = BuildList<CommandRequest>(Path.Combine(parentDir, "requests"));

            return config;
        }

        private static List<T> BuildList<T>(string folderPath)
        {
            var list = new List<T>();
            if (Directory.Exists(folderPath))
            {
                var files = Directory.GetFiles(folderPath, "*.json");
                foreach (var filePath in files)
                {
                    var content = File.ReadAllText(filePath);
                    if (content.Length > 0)
                    {
                        bool isArray = false;

                        for (int i = 0; i < content.Length; i++)
                        {
                            if (!char.IsWhiteSpace(content[i]))
                            {
                                isArray = content[i] == '[';
                                break;
                            }
                        }
                        
                        if (isArray)
                        {
                            var obj = JsonConvert.DeserializeObject<List<T>>(content, new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.Auto
                            });

                            list.AddRange(obj);
                        }
                        else
                        {
                            var obj = JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.Auto
                            });

                            list.Add(obj);
                        }
                    }
                }
            }

            return list;
        }
    }
}
