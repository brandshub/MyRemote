using MyRemote.Lib;
using MyRemote.Lib.Action;
using MyRemote.Lib.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MyRemote.AndroidClient.Business
{
    public static class Globals
    {
        public static List<ServerCredentials> SavedServers { get; set; } = new List<ServerCredentials>();
        public static bool AutoloadLastConfig { get; set; }

        public static Config CurrentConfig { get; set; }


        public static CommandAction FindActionById(string id)
        {
            if (CurrentConfig == null)
                return null;
            return CurrentConfig?.CommandActions.FirstOrDefault(p => p.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        public static void SaveServerData()
        {
            var path = System.IO.Path.Combine(FileSystem.AppDataDirectory, "servers.json");
            var json = JsonConvert.SerializeObject(SavedServers);

            File.WriteAllText(path, json);

        }

        public static async Task TryLoadDefaultData()
        {

            LoadServerData();
            if (SavedServers.Count > 0)
            {
                var server = GetSelectedOrFirstServer();
                if (server != null)
                {
                    try
                    {
                        CurrentConfig = await Server.GetConfigAsync(server.IpAddress, server.Port, server.Secret);
                        CurrentConfig.Server = new Server { IpAddress = server.IpAddress, Port = server.Port, Secret = server.Secret };
                    }
                    catch (Exception ex)
                    {
                        CurrentConfig = null;
                    }

                }

            }

        }
        public static void LoadServerData()
        {
            var path = System.IO.Path.Combine(FileSystem.AppDataDirectory, "servers.json");
            if (!File.Exists(path))
            {
                SavedServers = new List<ServerCredentials>();
                return;
            }

            var json = File.ReadAllText(path);
            SavedServers = JsonConvert.DeserializeObject<List<ServerCredentials>>(json);
        }


        public static ServerCredentials GetSelectedOrFirstServer()
        {
            if (SavedServers != null && SavedServers.Count > 0)
            {
                var selected = SavedServers.FirstOrDefault(s => s.Selected);
                if (selected == null)
                {
                    SavedServers[0].Selected = true;
                    return SavedServers[0];
                }
                return selected;
            }
            return null;
        }


    }
}
