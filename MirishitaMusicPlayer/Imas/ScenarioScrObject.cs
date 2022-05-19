using AssetStudio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;

namespace MirishitaMusicPlayer.Imas
{
    public class ScenarioScrObject
    {
        public ScenarioScrObject(MonoBehaviour monoBehaviour)
        {
            OrderedDictionary typeTree = monoBehaviour.ToType();

            foreach (var data in (List<object>)typeTree["scenario"])
            {
                Scenario.Add((EventScenarioData)Common.TypeTreeToType(typeof(EventScenarioData), data));
            }
        }

        public List<EventScenarioData> Scenario { get; } = new();
    }
}