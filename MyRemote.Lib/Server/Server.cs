using MyRemote.Lib.Command;
using MyRemote.Lib.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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


        public static async Task<CommandResponse> SendRequestAsync(Config config, CommandRequest request)
        {
            var timeOut = TimeSpan.FromSeconds(5);
            var cancellationCompletionSource = new TaskCompletionSource<bool>();

            using (var cts = new CancellationTokenSource(timeOut))
            {
                using (var client = new TcpClient())
                {
                    var task = client.ConnectAsync(config.Server.IpAddress, config.Server.Port);

                    using (cts.Token.Register(() => cancellationCompletionSource.TrySetResult(true)))
                    {
                        if (task != await Task.WhenAny(task, cancellationCompletionSource.Task))
                        {
                            throw new OperationCanceledException("TCP Connection timed out!", cts.Token);
                        }
                    }

                    if (!client.Connected)
                        throw new OperationCanceledException("TCP Connection failed!");

                    NetworkStream stream = client.GetStream();
                    BinaryWriter writer = new BinaryWriter(stream);
                   
                    var json = JsonConvert.SerializeObject(request);
                    writer.Write(json);
                    writer.Flush();

                    BinaryReader reader = new BinaryReader(stream);
                    var str = reader.ReadString();

                    // var resp = (CommandResponse)formatter.Deserialize(stream);
                    var resp = JsonConvert.DeserializeObject<CommandResponse>(str);
                    return resp;

                }
            }

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

        public static async Task<Config> GetConfigAsync(string ipAddress, int port, string secret)
        {
            var timeOut = TimeSpan.FromSeconds(5);
            var cancellationCompletionSource = new TaskCompletionSource<bool>();

            using (var cts = new CancellationTokenSource(timeOut))
            {
                using (var client = new TcpClient())
                {
                    var task = client.ConnectAsync(ipAddress, port);

                    using (cts.Token.Register(() => cancellationCompletionSource.TrySetResult(true)))
                    {
                        if (task != await Task.WhenAny(task, cancellationCompletionSource.Task))
                        {
                            throw new OperationCanceledException("TCP Connection timed out!", cts.Token);
                        }
                    }

                    if (!client.Connected)
                        throw new OperationCanceledException("TCP Connection failed!");

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
                    return config;

                }
            }
        }
    }
}
