using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MirishitaMusicPlayer.Forms
{
    public class EventLabel : Label
    {
        private readonly System.Timers.Timer animationTimer = new(1000f / 16f);

        public EventLabel() : base()
        {
            animationTimer.Interval = 16;
        }

        public void Trigger()
        {

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            animationTimer.Dispose();
        }
    }
}
