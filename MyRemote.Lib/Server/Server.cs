using MyRemote.Lib.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyRemote.Lib.Server
{
    public class Server
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public string Secret { get; set; }


        public static CommandResponse SendRequest(Config config, CommandRequest request)
        {
            var client = new TcpClient(config.Server.IpAddress, config.Server.Port);
            NetworkStream stream = client.GetStream();

            BinaryWriter writer = new BinaryWriter(stream);
            var json = JsonConvert.SerializeObject(request);
            writer.Write(json);
            writer.Flush();

            BinaryReader reader = new BinaryReader(stream);
            var jsonResp = reader.ReadString();
            var resp = JsonConvert.DeserializeObject<CommandResponse>(jsonResp);

            client.Close();
            return resp;
        }

        public static Config GetConfig(string ipAddress, int port, string secret)
        {
            var client = new TcpClient(ipAddress, port);
            NetworkStream stream = client.GetStream();

            BinaryWriter writer = new BinaryWriter(stream);

            var req = new CommandRequest();
            req.ActionId = "GET_CONFIG";
            req["secret"] = secret;

            var json = JsonConvert.SerializeObject(req);
            writer.Write(json);
            writer.Flush();

            BinaryReader reader = new BinaryReader(stream);
            var str = reader.ReadString();

            // var resp = (CommandResponse)formatter.Deserialize(stream);
            var resp = JsonConvert.DeserializeObject<CommandResponse>(str);


            var config = Config.FromString(resp.Payload["config"]);


            reader.Close();
            writer.Close();
            client.Close();

            return config;
        }
    }
}
