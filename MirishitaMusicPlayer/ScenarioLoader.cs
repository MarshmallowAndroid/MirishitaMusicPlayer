using AssetStudio;
using MirishitaMusicPlayer.Imas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MirishitaMusicPlayer
{
    internal enum Orientation
    {
        Yoko,   // Landscape mode
        Tate    // Portrait mode
    }

    internal class ScenarioLoader
    {
        public ScenarioLoader(
            AssetsManager assetsManager,
            string filesPath,
            string songID,
            Orientation orientation = Orientation.Tate)
        {
            // Load scenarios and notes first
            string scenarioPath = Path.Combine(filesPath, $"scrobj_{songID}.unity3d");
            assetsManager.LoadFiles(new[] { scenarioPath });

            MainScenario = null; // Main scenario for static events, like lyrics and lipsync
            OrientationScenario = null; // Landscape or portrait scenario, for unit or solo facial expressions and idol mute
            Notes = null; // Tap notes and song timing via EventConductor

            foreach (var file in assetsManager.assetsFileList)
            {
                foreach (var gameObject in file.Objects)
                {
                    if (gameObject.type == ClassIDType.MonoBehaviour)
                    {
                        // Find matching MonoBehaviour name and serialize into their corresponding types

                        MonoBehaviour monoBehaviour = (MonoBehaviour)gameObject;
                        if (monoBehaviour.m_Name == $"{songID}_scenario_sobj")
                            MainScenario = new(monoBehaviour);
                        else if (monoBehaviour.m_Name == $"{songID}_scenario_yoko_sobj" && orientation == Orientation.Yoko)
                            OrientationScenario = new(monoBehaviour);
                        else if (monoBehaviour.m_Name == $"{songID}_scenario_tate_sobj" && orientation == Orientation.Tate)
                            OrientationScenario = new(monoBehaviour);
                        else if (monoBehaviour.m_Name == $"{songID}_fumen_sobj")
                            Notes = new(monoBehaviour);
                    }
                }
            }

            // Figure out where the expression events are: in main scenario or orientation scenario?
            Func<EventScenarioData, bool> expressionPredicate = new(s => s.Type == ScenarioType.Expression);
            ExpressionScenarios = MainScenario.Scenario.Where(expressionPredicate).ToList();
            if (ExpressionScenarios.Count < 1) ExpressionScenarios = OrientationScenario.Scenario.Where(expressionPredicate).ToList();

            // Figure out where the mute events are: in main scenario or orientation scenario?
            Func<EventScenarioData, bool> mutePredicate = new(s => s.Type == ScenarioType.Mute);
            MuteScenarios = MainScenario.Scenario.Where(mutePredicate).ToList();
            if (MuteScenarios.Count < 1) MuteScenarios = OrientationScenario.Scenario.Where(mutePredicate).ToList();

            VoiceCount = MuteScenarios[0].Mute.Length;

            assetsManager.Clear();
        }

        public ScenarioScrObject MainScenario { get; }

        public ScenarioScrObject OrientationScenario { get; }

        public NoteScrObject Notes { get; }

        public List<EventScenarioData> ExpressionScenarios { get; }

        public List<EventScenarioData> MuteScenarios { get; }

        public int VoiceCount { get; }
    }
}