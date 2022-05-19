using AssetStudio;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace MirishitaMusicPlayer.Imas
{
    internal class NoteScrObject
    {
        public NoteScrObject(MonoBehaviour monoBehaviour)
        {
            OrderedDictionary typeDictionary = monoBehaviour.ToType();

            foreach (var data in (List<object>)typeDictionary["evts"])
            {
                Evts.Add((EventNoteData)Common.TypeTreeToType(typeof(EventNoteData), data));
            }

            foreach (var data in (List<object>)typeDictionary["ct"])
            {
                Ct.Add((EventConductorData)Common.TypeTreeToType(typeof(EventConductorData), data));
            }
        }

        public List<EventNoteData> Evts { get; } = new();

        public List<EventConductorData> Ct { get; } = new();
    }
}