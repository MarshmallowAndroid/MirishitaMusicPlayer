using AssetStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetStudioTest
{
    class EventNoteData
    {
        public double AbsTime { get; set; }

        public long Tick { get; set; }

        public int Track { get; set; }

        public int Type { get; set; }

        public float StartPosX { get; set; }

        public float EndPosX { get; set; }


    }

    class EventConductorData
    {

    }

    class NoteScrObject
    {
        public NoteScrObject(MonoBehaviour monoBehaviour)
        {

        }
    }
}
