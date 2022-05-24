using MirishitaMusicPlayer.Rgb;
using OpenRGB.NET;
using OpenRGB.NET.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MirishitaMusicPlayer.Forms
{
    public partial class RgbSettingsForm : Form
    {
        RgbManager manager;
        ZoneConfiguration currentColorConfiguration;

        public RgbSettingsForm(RgbManager rgbManager, List<int> targets)
        {
            InitializeComponent();

            manager = rgbManager;

            foreach (var item in targets)
            {
                targetComboBox.Items.Add(item);
            }

            RefreshDevices();
        }

        private void RefreshDevices()
        {
            deviceComboBox.Items.Clear();

            if (manager.ControllerConfigurations != null)
            {
                foreach (var device in manager.ControllerConfigurations)
                {
                    deviceComboBox.Items.Add(device);
                }
            }

            if (deviceComboBox.Items.Count > 0)
                deviceComboBox.SelectedIndex = 0;
        }

        private void DeviceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ControllerConfiguration selectedDevice = comboBox.SelectedItem as ControllerConfiguration;

            colorComboBox.Items.Clear();

            foreach (var item in selectedDevice.ZoneConfigurations)
            {
                colorComboBox.Items.Add(item);
            }

            if (colorComboBox.Items.Count > 0)
                colorComboBox.SelectedIndex = 0;
        }

        private void ColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZoneConfiguration colorConfiguration = colorComboBox.SelectedItem as ZoneConfiguration;

            currentColorConfiguration = colorConfiguration;

            targetComboBox.SelectedItem = colorConfiguration.PreferredTarget;
            colorSourceComboBox.SelectedIndex = colorConfiguration.PreferredSource;
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            manager.Connect();
            RefreshDevices();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            RefreshDevices();
        }

        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            manager.Disconnect();

            deviceComboBox.Items.Clear();
            colorComboBox.Items.Clear();
        }

        private void TargetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentColorConfiguration != null)
                currentColorConfiguration.PreferredTarget = (int)targetComboBox.SelectedItem;
        }

        private void ColorSourceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentColorConfiguration != null)
                currentColorConfiguration.PreferredSource = colorSourceComboBox.SelectedIndex;
        }
    }
}
