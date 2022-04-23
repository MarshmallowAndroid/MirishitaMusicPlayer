using MirishitaMusicPlayer.Imas;
using System;
using System.Text;

namespace MirishitaMusicPlayer
{
    internal static class MouthVisualizer
    {
        public static void Render(EventScenarioData scenario, int line)
        {
            if (scenario.Type == ScenarioType.LipSync)
            {
                StringBuilder mouthStringBuilder = new();

                switch (scenario.Param)
                {
                    case 0:
                        mouthStringBuilder.Append(
                            @"   ______________  " + "\n" +
                            @"   |            |  " + "\n" +
                            @"   |            |  " + "\n" +
                            @"   |            |  " + "\n" +
                            @"   \____________/  " + "\n" +
                            @"                   " + "\n");
                        break;

                    case 1:
                        mouthStringBuilder.Append(
                            @" __________________" + "\n" +
                            @" |\______________/|" + "\n" +
                            @" \  ____________  /" + "\n" +
                            @"  \/____________\/ " + "\n" +
                            @"                   " + "\n" +
                            @"                   " + "\n");
                        break;

                    case 2:
                        mouthStringBuilder.Append(
                            @"       ______      " + "\n" +
                            @"       |    |      " + "\n" +
                            @"       |____|      " + "\n" +
                            @"                   " + "\n" +
                            @"                   " + "\n" +
                            @"                   " + "\n");
                        break;

                    case 3:
                        mouthStringBuilder.Append(
                            @" __________________" + "\n" +
                            @" |\______________/|" + "\n" +
                            @" |                |" + "\n" +
                            @" \________________/" + "\n" +
                            @"                   " + "\n" +
                            @"                   " + "\n");
                        break;

                    case 4:
                        mouthStringBuilder.Append(
                            @"     __________    " + "\n" +
                            @"     |        |    " + "\n" +
                            @"     |        |    " + "\n" +
                            @"     |        |    " + "\n" +
                            @"     |________|    " + "\n" +
                            @"                   " + "\n");
                        break;

                    case 54:
                        mouthStringBuilder.Append(
                            @"                   " + "\n" +
                            @"     \________/    " + "\n" +
                            @"                   " + "\n" +
                            @"                   " + "\n" +
                            @"                   " + "\n" +
                            @"                   " + "\n");
                        break;

                    case 56:
                        mouthStringBuilder.Append(
                            @" __________________" + "\n" +
                            @" |\______________/|" + "\n" +
                            @" \/______________\/" + "\n" +
                            @"                   " + "\n" +
                            @"                   " + "\n" +
                            @"                   " + "\n");
                        break;

                    case 59:
                        mouthStringBuilder.Append(
                            @"     __________    " + "\n" +
                            @"     |\______/|    " + "\n" +
                            @"     | ______ |    " + "\n" +
                            @"     |/______\|    " + "\n" +
                            @"                   " + "\n" +
                            @"                   " + "\n");
                        break;

                    default:
                        mouthStringBuilder.Append(
                           @"                    " + "\n" +
                           @"    /\/\/\/\/\/\/\  " + "\n" +
                           @"                    " + "\n" +
                           @"                    " + "\n" +
                           @"                    " + "\n" +
                           @"                    " + "\n");
                        break;
                }

                Console.CursorLeft = 0;
                Console.CursorTop = line;
                Console.Write(mouthStringBuilder.ToString());
            }
        }
    }
}