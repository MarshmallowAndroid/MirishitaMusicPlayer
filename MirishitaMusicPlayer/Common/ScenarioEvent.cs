using MirishitaMusicPlayer.Imas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirishitaMusicPlayer.Common
{
    internal class ScenarioEvent
    {
        private int index = 0;
        private EventScenarioData currentScenarioData;
        private IList<EventScenarioData> scenarioDatas;

        private readonly ScenarioTriggeredCallback callback;

        public ScenarioEvent(IList<EventScenarioData> eventScenarioDatas, ScenarioTriggeredCallback scenarioTriggeredCallback)
        {
            currentScenarioData = eventScenarioDatas[index];
            scenarioDatas = eventScenarioDatas;
            callback = scenarioTriggeredCallback;
        }

        public void Update(double secondsElapsed)
        {
            while (currentScenarioData.AbsTime <= secondsElapsed)
            {
                callback?.Invoke(currentScenarioData);

                if (index < scenarioDatas.Count - 1) index++;
                else break;

                currentScenarioData = scenarioDatas[index];
            }
        }

        public void Seek(double secondsElapsed)
        {
            index = 0;
            currentScenarioData = scenarioDatas[index];

            if (secondsElapsed > 0)
            {
                while (currentScenarioData.AbsTime <= secondsElapsed)
                {
                    if (index < scenarioDatas.Count - 1) index++;
                    else break;
                    currentScenarioData = scenarioDatas[index];
                }

                callback?.Invoke(scenarioDatas[index - 1 > 0 ? index - 1 : 0]);
            }
            else
                callback?.Invoke(scenarioDatas[index]);
        }

        public void Reset()
        {
            index = 0;
            currentScenarioData = scenarioDatas[index];
        }

        public delegate void ScenarioTriggeredCallback(EventScenarioData scenarioData);
    }
}
