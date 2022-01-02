﻿using MyRemote.Lib.Action;
using MyRemote.Lib.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MyRemote.Server
{
    public class ClientObject
    {
        public TcpClient client;
        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
        }

        public void Process()
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                CommandResponse response = null;

                BinaryReader reader = new BinaryReader(stream);
                var reqJson = reader.ReadString();

                bool error = false;


                CommandRequest request = JsonConvert.DeserializeObject<CommandRequest>(reqJson);
                if (request.ActionId == "GET_CONFIG")
                {
                    if (request.Parameters["secret"] == Common.CurrentConfig.Server.Secret)
                    {
                        string currentConfigJson = Common.CurrentConfig.ToJson();
                        response = new CommandResponse
                        {
                            Payload = new Dictionary<string, string> { { "config", currentConfigJson } }
                        };
                    }
                    else
                    {
                        response = new CommandResponse { StatusCode = -100 };
                    }
                }
                else
                {
                    var action = CommandActionFactory.FromRequest(request, Common.CurrentConfig);
                    try
                    {
                        response = action.Execute();
                    }
                    catch (Exception exz)
                    {
                        response = new CommandResponse { StatusCode = -200, Payload = new Dictionary<string, string> { { "err", exz.Message } } };
                    }
                }


                var respJson = JsonConvert.SerializeObject(response);

                BinaryWriter bw = new BinaryWriter(stream);
                bw.Write(respJson);
                bw.Flush();


                /*
                var formatter = new BinaryFormatter();
                CommandRequest request = (CommandRequest)formatter.Deserialize(stream);

               

                formatter.Serialize(stream, response);*/




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }
    }
}
