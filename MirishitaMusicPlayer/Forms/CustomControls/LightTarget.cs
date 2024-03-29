﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MirishitaMusicPlayer.Animation;
using MirishitaMusicPlayer.Imas;

namespace MirishitaMusicPlayer.Forms.CustomControls
{
    public partial class LightTarget : UserControl
    {
        private readonly int defaultHeight;

        public LightTarget()
        {
            InitializeComponent();

            defaultHeight = Height;
        }

        public LightTarget(int lightTarget) : this()
        {
            Target = lightTarget;

            targetLabel.Text = lightTarget.ToString();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;

                return cp;
            }
        }

        public int Target { get; }

        public bool HideLabel
        {
            get
            {
                return targetLabel.Visible;
            }
            set
            {
                targetLabel.Visible = value;

                if (!value)
                    Height = defaultHeight - 64;
                else
                    Height = defaultHeight;
            }
        }

        public async Task UpdateColors(
            ColorRGBA color1,
            ColorRGBA color2,
            ColorRGBA color3,
            float duration)
        {
            await lightLabel1.FadeBackColor(color1?.ToColor() ?? System.Drawing.Color.Black, duration);
            await lightLabel2.FadeBackColor(color2?.ToColor() ?? System.Drawing.Color.Black, duration);
            await lightLabel3.FadeBackColor(color3?.ToColor() ?? System.Drawing.Color.Black, duration);
        }
    }
}
