using MyRemote.Lib;
using NLog;
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
    public partial class MainForm : Form
    {
        private static ILogger logger;

        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Hide();
        }

        private static TcpListener listener;
        private void button1_Click(object sender, EventArgs e)
        {
            var cfg = Common.CurrentConfig;

            if (!IPAddress.TryParse(cfg.Server.IpAddress, out IPAddress ipAddress))
            {
                ipAddress = Helper.GetLocalIPAddress();
                cfg.Server.IpAddress = ipAddress.ToString();
            }

            logger.Info("Starting server");

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

        private void Form1_Load(object sender, EventArgs e)
        {


            if (logger == null)
                logger = LogManager.GetCurrentClassLogger();

            button1_Click(null, EventArgs.Empty);
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void rtbLog_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void hideIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            Activate();
            hideIcon.Visible = false;
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {

            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                hideIcon.Visible = true;
            }

        }
    }
}
