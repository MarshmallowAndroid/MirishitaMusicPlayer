using AssetStudio;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace MirishitaMusicPlayer.Imas
{
    class NoteScrObject
    {
        public NoteScrObject(MonoBehaviour monoBehaviour)
        {
            OrderedDictionary typeDictionary = monoBehaviour.ToType();

            foreach (var data in (List<object>)typeDictionary["evts"])
            {
                EventNoteData eventData = new();

                foreach (DictionaryEntry item in (OrderedDictionary)data)
                {
                    switch (item.Key)
                    {
                        case "absTime":
                            eventData.AbsTime = (double)item.Value;
                            break;
                        case "tick":
                            eventData.Tick = (long)item.Value;
                            break;
                        case "track":
                            eventData.Track = (int)item.Value;
                            break;
                        case "type":
                            eventData.Type = (int)item.Value;
                            break;
                        default:
                            break;
                    }
                }

                Evts.Add(eventData);
            }

            foreach (var data in (List<object>)typeDictionary["ct"])
            {
                EventConductorData eventData = new();

                foreach (DictionaryEntry item in (OrderedDictionary)data)
                {
                    switch (item.Key)
                    {
                        case "tempo":
                            eventData.Tempo = (double)item.Value;
                            break;
                        case "tsigNumerator":
                            eventData.TSigNumerator = (int)item.Value;
                            break;
                        case "tsigDenominator":
                            eventData.TSigDenominator = (int)item.Value;
                            break;
                        default:
                            break;
                    }
                }

                Ct.Add(eventData);
            }
        }

        public List<EventNoteData> Evts { get; } = new();

        public List<EventConductorData> Ct { get; } = new();
    }
}

