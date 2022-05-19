using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MirishitaMusicPlayer.Imas;
using Color = MirishitaMusicPlayer.Imas.Color;

namespace MirishitaMusicPlayer.Forms
{
    public partial class LightTarget : UserControl
    {
        public LightTarget()
        {
            InitializeComponent();
        }

        public LightTarget(int lightTarget) : this()
        {
            Target = lightTarget;

            targetLabel.Text = lightTarget.ToString();
        }

        public int Target { get; }

        public void UpdateColors(
            Color color1,
            Color color2,
            Color color3,
            double duration)
        {
            if (color1 != null)
                lightLabel1.FadeBackColor(color1.ToColor(), duration);
            else
                lightLabel1.Visible = false;

            if (color2 != null)
                lightLabel2.FadeBackColor(color2.ToColor(), duration);
            else
                lightLabel2.Visible = false;

            if (color3 != null)
                lightLabel3.FadeBackColor(color3.ToColor(), duration);
            else
                lightLabel3.Visible = false;
        }
    }
}
