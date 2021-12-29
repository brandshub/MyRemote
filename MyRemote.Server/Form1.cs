using MyRemote.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyRemote.Server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            button1_Click(null, EventArgs.Empty);

        }

        
        static TcpListener listener;
        private void button1_Click(object sender, EventArgs e)
        {
            var cfg = Common.CurrentConfig;

            if(!IPAddress.TryParse(cfg.Server.IpAddress, out IPAddress ipAddress))
            {
                ipAddress = Helper.GetLocalIPAddress();
                cfg.Server.IpAddress = ipAddress.ToString();                
            }

            textBox1.AppendText("starting\r\n");
            Task.Run(() =>
            {
                try
                {
                    listener = new TcpListener(ipAddress, cfg.Server.Port);
                    listener.Start();


                    while (true)
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        ClientObject clientObject = new ClientObject(client);

                        textBox1.AppendText("incoming\r\n");
                        Task clientTask = new Task(clientObject.Process);
                        clientTask.Start();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (listener != null)
                        listener.Stop();
                }
            });
        }
    }
}
