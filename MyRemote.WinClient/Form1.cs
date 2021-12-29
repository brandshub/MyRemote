using MyRemote.Lib;
using MyRemote.Lib.Action;
using MyRemote.Lib.Command;
using MyRemote.Lib.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyRemote.WinClient
{
    public partial class Form1 : Form
    {
        private Config cfg;
        public Form1()
        {
            InitializeComponent();
            cfg = Config.LoadFromFile();

        }




        private void button1_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                var client = new TcpClient(cfg.Server.IpAddress, cfg.Server.Port);

                NetworkStream stream = client.GetStream();

                var req = new CommandRequest();
                req.ActionId = RunProcessAction.CODE;
                req.Parameters = new Dictionary<string, string> { { RunProcessAction.COMMAND, textBox1.Text } };


                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, req);

                //                BinaryReader reader = new BinaryReader(stream);
                var resp = (CommandResponse)formatter.Deserialize(stream);


                //              reader.Close();
                //            writer.Close();
                client.Close();
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                Server.SendRequest(cfg, cfg.Requests.FirstOrDefault(d => d.ActionId == "LIST_TORRENTS"));
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                var client = new TcpClient("192.168.0.101", 5006);
                NetworkStream stream = client.GetStream();

                var req = new CommandRequest();
                req.ActionId = KeyboardAction.CODE;

                req.Parameters = new Dictionary<string, string> { { KeyboardAction.INPUT, textBox1.Text } };

                await Task.Delay(2000);

                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, req);

                //                BinaryReader reader = new BinaryReader(stream);
                var resp = (CommandResponse)formatter.Deserialize(stream);


                //              reader.Close();
                //            writer.Close();
                client.Close();
            });
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                var client = new TcpClient("192.168.0.101", 5006);
                NetworkStream stream = client.GetStream();

                var req = new CommandRequest();
                req.ActionId = textBox1.Text;

                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, req);

                //                BinaryReader reader = new BinaryReader(stream);
                var resp = (CommandResponse)formatter.Deserialize(stream);


                //              reader.Close();
                //            writer.Close();
                client.Close();
            });
        }

        private void btnGetConfig_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {

                var config = Server.GetConfig("192.168.0.1", 5006, "1234");
                /*
                var client = new TcpClient("192.168.0.101", 5006);
                NetworkStream stream = client.GetStream();

                var req = new CommandRequest();
                req.ActionId = "GET_CONFIG";
                req["secret"] = textBox1.Text;

                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, req);

                //                BinaryReader reader = new BinaryReader(stream);
                var resp = (CommandResponse)formatter.Deserialize(stream);


                //              reader.Close();
                //            writer.Close();
                client.Close();*/
            });
        }
    }
}
