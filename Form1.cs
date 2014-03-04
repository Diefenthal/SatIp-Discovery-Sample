using System;
using System.Windows.Forms;
using SatIp.DiscoverySample.Upnp;

namespace SatIp.DiscoverySample
{
    public partial class Form1 : Form
    {
        private readonly UpnpClient _client = new UpnpClient();
        public Form1()
        {
            InitializeComponent();
            
            var devices=_client.Search(500);

            foreach (var device in devices)
            {
                listBox1.Items.Add(device);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var device = (UpnpDevice) listBox1.SelectedItem;

            textBox1.Text = device.FriendlyName;
            textBox2.Text = device.UDN;
            textBox3.Text = device.DeviceType;
            textBox4.Text = device.ModelDescription;
            textBox5.Text = device.Manufacturer;
            textBox6.Text = device.ManufacturerUrl;
            textBox7.Text = device.PresentationUrl;
            webBrowser1.DocumentText = device.DeviceDescription;
            try
            {
                string imageUrl = string.Format(device.FriendlyName== "OctopusNet" ? "http://{0}:{1}/{2}" : "http://{0}:{1}{2}", device.BaseHost, device.BasePort, device.GetImage(1));
                pictureBox1.LoadAsync(imageUrl);
                pictureBox1.Visible = true;
            }
            catch (Exception)
            {
               pictureBox1.Visible = false;
            }
        }
    }
}
