using AssetStudio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace AssetStudioTest
{
    class EventScenarioData
    {
        public double AbsTime { get; set; }

        public long Tick { get; set; }

        public int Type { get; set; }

        public int Param { get; set; }

        public string Str { get; set; }

        public int Idol { get; set; }

        public byte[] Mute { get; set; }
    }

    class ScenarioScrObject
    {
        OrderedDictionary typeDictionary;

        public ScenarioScrObject(MonoBehaviour monoBehaviour)
        {
            typeDictionary = monoBehaviour.ToType();

            foreach (var scenarios in (List<object>)typeDictionary["scenario"])
            {
                EventScenarioData eventData = new();

                foreach (DictionaryEntry item in (OrderedDictionary)scenarios)
                {
                    //Console.WriteLine(item.Key);
                    switch (item.Key)
                    {
                        case "absTime":
                            eventData.AbsTime = (double)item.Value;
                            break;
                        case "tick":
                            eventData.Tick = (long)item.Value;
                            break;
                        case "type":
                            eventData.Type = (int)item.Value;
                            break;
                        case "param":
                            eventData.Param = (int)item.Value;
                            break;
                        case "str":
                            eventData.Str = (string)item.Value;
                            break;
                        case "idol":
                            eventData.Idol = (int)item.Value;
                            break;
                        case "mute":
                            var mute = (List<object>)item.Value;
                            if (mute.Count > 0)
                            {
                                eventData.Mute = new byte[mute.Count];
                                for (int i = 0; i < mute.Count; i++)
                                {
                                    eventData.Mute[i] = (byte)mute[i];
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }

                Events.Add(eventData);
            }
        }

        List<EventScenarioData> Events { get; } = new();
    }
}
