using AssetStudio;
using MirishitaMusicPlayer.Imas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MirishitaMusicPlayer.Common
{
    public class SongScenario
    {
        public SongScenario(
            string scenarioFile,
            Song song,
            AssetsManager assetsManager,
            ScenarioOrientation orientation = ScenarioOrientation.Tate)
        {
            // Load scenarios and notes first
            assetsManager.LoadFiles(new[] { scenarioFile });

            MainScenario = null; // Main scenario for static events, like lyrics and lipsync
            OrientationScenario = null; // Landscape or portrait scenario, for unit or solo facial expressions and idol mute
            //Notes = null; // Tap notes and song timing via EventConductor

            foreach (var file in assetsManager.assetsFileList)
            {
                foreach (var gameObject in file.Objects)
                {
                    if (gameObject.type == ClassIDType.MonoBehaviour)
                    {
                        // Find matching MonoBehaviour name and serialize into their corresponding types

                        MonoBehaviour monoBehaviour = (MonoBehaviour)gameObject;
                        if (monoBehaviour.m_Name.EndsWith("_scenario_sobj"))
                            MainScenario = new(monoBehaviour);
                        else if (monoBehaviour.m_Name.EndsWith("_scenario_yoko_sobj") && orientation == ScenarioOrientation.Yoko)
                            OrientationScenario = new(monoBehaviour);
                        else if (monoBehaviour.m_Name.EndsWith("_scenario_tate_sobj") && orientation == ScenarioOrientation.Tate)
                            OrientationScenario = new(monoBehaviour);
                        //else if (monoBehaviour.m_Name == $"{songID}_fumen_sobj")
                        //    Notes = new(monoBehaviour);
                    }
                }
            }

            ExpressionScenarios = FindScenarios(ScenarioType.Expression);
            MuteScenarios = FindScenarios(ScenarioType.Mute);
            LipSyncScenarios = FindScenarios(ScenarioType.LipSync);
            LightScenarios = FindScenarios(ScenarioType.Lights);

            StageMemberCount = MuteScenarios[0].Mute.Length;

            assetsManager.Clear();

            Configuration = new(song, this, assetsManager);
        }

        private List<EventScenarioData> FindScenarios(ScenarioType scenarioType)
        {
            // Figure out where the events are: in main scenario or orientation scenario?
            Func<EventScenarioData, bool> predicate = new(s => s.Type == scenarioType);
            List<EventScenarioData> scenarios = OrientationScenario.Scenario.Where(predicate).ToList();
            if (scenarios.Count < 1) scenarios = MainScenario.Scenario.Where(predicate).ToList();

            return scenarios;
        }

        public int StageMemberCount { get; }

        public ScenarioScrObject MainScenario { get; }

        public ScenarioScrObject OrientationScenario { get; }

        public List<EventScenarioData> ExpressionScenarios { get; }

        public List<EventScenarioData> MuteScenarios { get; }

        public List<EventScenarioData> LipSyncScenarios { get; }

        public List<EventScenarioData> LightScenarios { get; }

        public SongScenarioConfiguration Configuration { get; }
    }
}
