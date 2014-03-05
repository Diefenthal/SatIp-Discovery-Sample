/*  
    Copyright (C) <2007-2014>  <Kay Diefenthal>

    SatIp.DiscoverySample is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    SatIp.DiscoverySample is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with SatIp.DiscoverySample.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Windows.Forms;
using SatIp.DiscoverySample.Logging;
using SatIp.DiscoverySample.Properties;
using SatIp.DiscoverySample.Upnp;

namespace SatIp.DiscoverySample
{
    public partial class Form1 : Form
    {
        private readonly UpnpClient _client = new UpnpClient();
        public Form1()
        {
            InitializeComponent();
            Logger.SetLogFilePath("Sample.log", Settings.Default.LogLevel);
            Logger.Info("Search for Transmitters");
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
