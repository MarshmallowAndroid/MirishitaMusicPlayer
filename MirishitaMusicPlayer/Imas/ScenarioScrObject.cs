using AssetStudio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;

namespace MirishitaMusicPlayer.Imas
{
    internal class ScenarioScrObject
    {
        public ScenarioScrObject(MonoBehaviour monoBehaviour)
        {
            OrderedDictionary typeDictionary = monoBehaviour.ToType();

            foreach (var data in (List<object>)typeDictionary["scenario"])
            {
                EventScenarioData eventData = new();

                foreach (DictionaryEntry dataProperty in (OrderedDictionary)data)
                {
                    Type eventDataType = eventData.GetType();

                    PropertyInfo matchingProperty = eventDataType.GetProperties()
                        .FirstOrDefault(p => p.Name.ToLower().Equals(dataProperty.Key.ToString().ToLower()));

                    if (matchingProperty != null)
                    {
                        object value = null;
                        if (dataProperty.Value.GetType() == typeof(List<object>))
                        {
                            List<object> list = (List<object>)dataProperty.Value;

                            Type elementType;
                            if (list?.Count > 0)
                            {
                                elementType = list[0].GetType();

                                Array newArray = Array.CreateInstance(elementType, list.Count);
                                list.ToArray().CopyTo(newArray, 0);

                                value = newArray;
                            }
                        }
                        else
                        {
                            value = dataProperty.Value;
                        }

                        matchingProperty.SetValue(eventData, value);
                    }
                }

                Scenario.Add(eventData);
            }
        }

        public List<EventScenarioData> Scenario { get; } = new();
    }
}