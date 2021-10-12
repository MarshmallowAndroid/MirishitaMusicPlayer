using AssetStudio;
using MirishitaMusicPlayer.Imas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirishitaMusicPlayer
{
    enum Orientation
    {
        Yoko,   // Landscape mode
        Tate    // Portrait mode
    }

    class ScenarioLoader
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

            MainScenario = null;
            OrientationScenario = null;
            Notes = null; // Tap notes and song timing via EventConductor

            foreach (var file in assetsManager.assetsFileList)
            {
                foreach (var gameObject in file.Objects)
                {
                    if (gameObject.type == ClassIDType.MonoBehaviour)
                    {
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

            Func<EventScenarioData, bool> mutePredicate = new Func<EventScenarioData, bool>(s => s.Type == ScenarioType.Mute);
            MuteScenarios = MainScenario.Scenario.Where(mutePredicate).ToList();
            if (MuteScenarios.Count < 1) MuteScenarios = OrientationScenario.Scenario.Where(mutePredicate).ToList();

            VoiceCount = MuteScenarios[0].Mute.Length;

            //List<object> exist = new();
            //foreach (var scenario in tateScenario.Scenario.Where(s => s.Type == ScenarioType.Expression))
            //{
            //    object target = scenario.Param;
            //    if (!exist.Contains(target))
            //    {
            //        exist.Add(target);
            //        Console.WriteLine(target);
            //    }
            //}
            //Console.WriteLine();

            assetsManager.Clear();
        }

        public ScenarioScrObject MainScenario { get; }

        public ScenarioScrObject OrientationScenario { get; }

        public List<EventScenarioData> MuteScenarios { get; }

        public NoteScrObject Notes { get; }

        public int VoiceCount { get; }

        public float TicksPerSecond => (float)(Notes.Ct[0].Tempo * (Notes.Ct[0].TSigNumerator + Notes.Ct[0].TSigDenominator));
    }
}
