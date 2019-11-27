using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceProcess;


namespace SmartNamePlate.PCCompanion
{
    public partial class DebugMain : Form
    {
        private WindowsStatusHubClient client;
        private readonly string hubURL;
        public DebugMain()
        {
            InitializeComponent();
            hubURL = System.Configuration.ConfigurationManager.AppSettings["hubUrl"];
            btnStop.Enabled = false;
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (client == null)
            {
                client = new WindowsStatusHubClient(hubURL);
                await client.Connect();
                btnStart.Enabled = false;
                btnStop.Enabled = true;
            }
        }

        private async void btnStop_Click(object sender, EventArgs e)
        {
            if (client != null)
            {
                await client.Disconnect();
                client = null;
                btnStart.Enabled = true;
                btnStop.Enabled = false;
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (client != null)
            {
                await client.Send(cboEvents.Text);
            }
        }
    }
}
