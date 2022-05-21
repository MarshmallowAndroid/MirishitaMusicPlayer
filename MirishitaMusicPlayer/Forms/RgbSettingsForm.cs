using OpenRGB.NET;
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
        private readonly IOpenRGBClient client;

        public RgbSettingsForm(IOpenRGBClient rgbClient)
        {
            InitializeComponent();

            client = rgbClient;
        }
    }
}
