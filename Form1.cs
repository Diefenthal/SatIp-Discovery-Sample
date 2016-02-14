/*  
    Copyright (C) <2007-2016>  <Kay Diefenthal>

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
        private readonly SSDPClient _client = new SSDPClient();
        public Form1()
        {
            InitializeComponent();
            Logger.SetLogFilePath("Sample.log", Settings.Default.LogLevel);
            Logger.Info("Search for Transmitters");
            _client.DeviceFound += DeviceFound;
            _client.DeviceLost += DeviceLost;           
            _client.Start();
            _client.FindByType();
        }

        private void DeviceFound(object sender, SatIpDeviceFoundArgs args)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    DeviceFound(sender, args);
                });
                return;
            }
            var newnode = treeView1.Nodes[0].Nodes.Add(args.Device.UniqueDeviceName, args.Device.FriendlyName);
            newnode.ToolTipText = args.Device.DeviceDescription;
            if (treeView1.Nodes[0].IsExpanded != true)
                treeView1.Nodes[0].Expand();

        }
        private void DeviceLost(object sender, SatIpDeviceLostArgs args)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    DeviceLost(sender, args);
                });
                return;
            }
            Logger.Info("Device with UUID :{0} restarts,and will removed from the Devices Tree", args.Uuid);
            if (treeView1.Nodes[0].Nodes.ContainsKey(args.Uuid))
            {
                var tn = treeView1.Nodes[0].Nodes[args.Uuid];
                treeView1.Nodes[0].Nodes.Remove(tn);
                treeView1.Update();
            }
        }
        
        
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var device = (SatIpDevice)_client.FindByUDN(e.Node.Name);
            if (device != null)
            {
                textBox1.Text = device.FriendlyName;
                textBox2.Text = device.UniqueDeviceName;
                textBox3.Text = device.DeviceType;
                textBox4.Text = device.ModelDescription;
                textBox5.Text = device.Manufacturer;
                textBox6.Text = device.ManufacturerUrl;
                textBox7.Text = device.PresentationUrl;
                webBrowser1.DocumentText = device.DeviceDescription;
                try
                {
                    string imageUrl = string.Format(device.FriendlyName == "OctopusNet" ? "http://{0}:{1}/{2}" : "http://{0}:{1}{2}", device.BaseUrl.Host, device.BaseUrl.Port, device.GetImage(1));

                    pictureBox1.LoadAsync(imageUrl);
                    pictureBox1.Visible = true;
                }
                catch (Exception)
                {
                    pictureBox1.Visible = false;
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _client.DeviceFound -= DeviceFound;
            _client.DeviceLost -= DeviceLost;
            _client.Dispose();
        }
    }
}
