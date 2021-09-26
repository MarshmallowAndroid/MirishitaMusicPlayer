using System.Collections.Generic;
using AssetStudio;
using System.Collections;
using System.Collections.Specialized;

namespace MirishitaMusicPlayer.Imas
{
    class ScenarioScrObject
    {
        public ScenarioScrObject(MonoBehaviour monoBehaviour)
        {
            OrderedDictionary typeDictionary = monoBehaviour.ToType();

            foreach (var data in (List<object>)typeDictionary["scenario"])
            {
                EventScenarioData eventData = new();

                foreach (DictionaryEntry property in (OrderedDictionary)data)
                {
                    switch (property.Key)
                    {
                        case "absTime":
                            eventData.AbsTime = (double)property.Value;
                            break;
                        case "tick":
                            eventData.Tick = (long)property.Value;
                            break;
                        case "track":
                            eventData.Track = (int)property.Value;
                            break;
                        case "type":
                            eventData.Type = (ScenarioType)property.Value;
                            break;
                        case "param":
                            eventData.Param = (int)property.Value;
                            break;
                        case "target":
                            eventData.Target = (int)property.Value;
                            break;
                        case "str":
                            eventData.Str = (string)property.Value;
                            break;
                        case "idol":
                            eventData.Idol = (int)property.Value;
                            break;
                        case "camNo":
                            eventData.CamNo = (int)property.Value;
                            break;
                        case "mute":
                            var mute = (List<object>)property.Value;
                            if (mute.Count > 0)
                            {
                                eventData.Mute = new byte[mute.Count];
                                for (int i = 0; i < mute.Count; i++)
                                {
                                    eventData.Mute[i] = (byte)mute[i];
                                }
                            }
                            break;
                        case "eyeclose":
                            eventData.EyeClose = (byte)property.Value;
                            break;
                        case "seekFrame":
                            eventData.SeekFrame = (int)property.Value;
                            break;
                        case "idol2":
                            eventData.Idol2 = (int)property.Value;
                            break;
                        default:
                            break;
                    }
                }

                Scenario.Add(eventData);
            }
        }

        public List<EventScenarioData> Scenario { get; } = new();
    }
}

