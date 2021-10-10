using MirishitaMusicPlayer.Imas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirishitaMusicPlayer
{
    static class MouthVisualizer
    {
        public static void Render(EventScenarioData scenario, int line)
        {
            if (scenario.Type == ScenarioType.LipSync)
            {
                //if (mouthCursorTop < 0) mouthCursorTop = Console.CursorTop;
                Console.CursorLeft = 0;
                Console.CursorTop = line;

                //Console.WriteLine(" [" + current.Param + "] ");
                //Console.WriteLine("Type: " + current.Type + ", Parameter: " + current.Param + "        ");
                //Console.WriteLine();
                StringBuilder mouthStringBuilder = new();

                switch (scenario.Param)
                {
                    case 0:
                        mouthStringBuilder.Append(
                            @"   ______________  " + "\n" +
                            @"   |            |  " + "\n" +
                            @"   |            |  " + "    Sound: A, ID: " + scenario.Param + "        \n" +
                            @"   |            |  " + "\n" +
                            @"   \____________/  " + "\n" +
                            @"                   " + "\n");
                        break;
                    case 1:
                        mouthStringBuilder.Append(
                            @" __________________" + "\n" +
                            @" |\______________/|" + "\n" +
                            @" \  ____________  /" + "    Sound: I, ID: " + scenario.Param + "        \n" +
                            @"  \/____________\/ " + "\n" +
                            @"                   " + "\n" +
                            @"                   " + "\n");
                        break;
                    case 2:
                        mouthStringBuilder.Append(
                            @"       ______      " + "\n" +
                            @"       |    |      " + "\n" +
                            @"       |____|      " + "    Sound: U, ID: " + scenario.Param + "        \n" +
                            @"                   " + "\n" +
                            @"                   " + "\n" +
                            @"                   " + "\n");
                        break;
                    case 3:
                        mouthStringBuilder.Append(
                            @" __________________" + "\n" +
                            @" |\______________/|" + "\n" +
                            @" |                |" + "    Sound: E, ID: " + scenario.Param + "        \n" +
                            @" \________________/" + "\n" +
                            @"                   " + "\n" +
                            @"                   " + "\n");
                        break;
                    case 4:
                        mouthStringBuilder.Append(
                            @"     __________    " + "\n" +
                            @"     |        |    " + "\n" +
                            @"     |        |    " + "    Sound: O, ID: " + scenario.Param + "        \n" +
                            @"     |        |    " + "\n" +
                            @"     |________|    " + "\n" +
                            @"                   " + "\n");
                        break;
                    case 54:
                        mouthStringBuilder.Append(
                            @"                   " + "\n" +
                            @"     \________/    " + "\n" +
                            @"                   " + "    Sound: -, ID: " + scenario.Param + "        \n" +
                            @"                   " + "\n" +
                            @"                   " + "\n" +
                            @"                   " + "\n");
                        break;
                    case 56:
                        mouthStringBuilder.Append(
                            @" __________________" + "\n" +
                            @" |\______________/|" + "\n" +
                            @" \/______________\/" + "    Sound: I, ID: " + scenario.Param + "        \n" +
                            @"                   " + "\n" +
                            @"                   " + "\n" +
                            @"                   " + "\n");
                        break;
                    case 59:
                        mouthStringBuilder.Append(
                            @"     __________    " + "\n" +
                            @"     |\______/|    " + "\n" +
                            @"     | ______ |    " + "    Sound: O, ID: " + scenario.Param + "        \n" +
                            @"     |/______\|    " + "\n" +
                            @"                   " + "\n" +
                            @"                   " + "\n");
                        break;
                    default:
                        mouthStringBuilder.Append(
                           @"                    " + "\n" +
                           @"    /\/\/\/\/\/\/\  " + "\n" +
                           @"                    " + "    Unknown shape: " + scenario.Param + "        \n" +
                           @"                    " + "\n" +
                           @"                    " + "\n" +
                           @"                    " + "\n");
                        break;
                }

                Console.Write(mouthStringBuilder.ToString());
            }
        }
    }
}
