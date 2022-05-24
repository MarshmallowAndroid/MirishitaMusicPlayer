﻿using MirishitaMusicPlayer.RgbPluginBase;
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

namespace SimpleRgbPlugin
{
    public partial class RgbSettingsForm : Form
    {
        private readonly IRgbManager manager;
        private LedConfiguration currentColorConfiguration;

        public RgbSettingsForm(SimpleRgbManager rgbManager, IEnumerable<int> targets)
        {
            InitializeComponent();

            manager = rgbManager;

            targetComboBox.Items.Add("None");
            foreach (var item in targets)
            {
                targetComboBox.Items.Add(item);
            }

            RefreshDevices();
        }

        private void RefreshDevices()
        {
            deviceComboBox.Items.Clear();

            if (!manager.Connect()) Close();
            if (manager.DeviceConfigurations != null)
            {
                foreach (var device in manager.DeviceConfigurations)
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
            DeviceConfiguration selectedDevice = comboBox.SelectedItem as DeviceConfiguration;

            ledComboBox.Items.Clear();

            foreach (var item in selectedDevice.LedConfigurations)
            {
                ledComboBox.Items.Add(item);
            }

            if (ledComboBox.Items.Count > 0)
                ledComboBox.SelectedIndex = 0;
        }

        private void LedComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LedConfiguration colorConfiguration = ledComboBox.SelectedItem as LedConfiguration;

            currentColorConfiguration = colorConfiguration;

            if (colorConfiguration.PreferredTarget == -1)
                targetComboBox.SelectedIndex = 0;
            else
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
            ledComboBox.Items.Clear();
        }

        private void TargetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentColorConfiguration != null)
            {
                if (targetComboBox.SelectedIndex == 0)
                    currentColorConfiguration.PreferredTarget = -1;
                else
                    currentColorConfiguration.PreferredTarget = (int)targetComboBox.SelectedItem;
            }
        }

        private void ColorSourceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentColorConfiguration != null)
                currentColorConfiguration.PreferredSource = colorSourceComboBox.SelectedIndex;
        }
    }
}