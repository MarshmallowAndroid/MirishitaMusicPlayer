using MirishitaMusicPlayer.Imas;
using System;
using System.Text;

namespace MirishitaMusicPlayer
{
    internal static class EyesVisualizer
    {
        public static void Render(EventScenarioData scenario, int line)
        {
            if (scenario.Type == ScenarioType.Expression)
            {
                if (scenario.Idol == 0)
                {
                    StringBuilder eyesStringBuilder = new();

                    switch (scenario.Param)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            eyesStringBuilder.Append(
                                @"   ___        ___  " + "\n" +
                                @" --              --" + "\n" +
                                @"  ---          --- " + "\n" +
                                @" | O |        | O |" + "\n" +
                                @" -----        -----" + "\n" +
                                @"                   " + "\n");
                            break;

                        case 4:
                            eyesStringBuilder.Append(
                                @"    __        __   " + "\n" +
                                @" ---            ---" + "\n" +
                                @"  ---          --- " + "\n" +
                                @" | O |        | O |" + "\n" +
                                @" -----        -----" + "\n" +
                                @"                   " + "\n");
                            break;

                        case 5:
                            eyesStringBuilder.Append(
                                @"   ___        ___  " + "\n" +
                                @" --              --" + "\n" +
                                @"  ___          ___ " + "\n" +
                                @" -   -        -   -" + "\n" +
                                @"                   " + "\n" +
                                @"                   " + "\n");
                            break;

                        case 8:
                        case 9:
                            eyesStringBuilder.Append(
                                @"                   " + "\n" +
                                @" ----_        _----" + "\n" +
                                @"  ---          --- " + "\n" +
                                @" | O |        | O |" + "\n" +
                                @" -----        -----" + "\n" +
                                @"                   " + "\n");
                            break;

                        case 10:
                        case 16:
                            eyesStringBuilder.Append(
                                @"                   " + "\n" +
                                @" ---__        __---" + "\n" +
                                @"  ---          --- " + "\n" +
                                @" | O |        | O |" + "\n" +
                                @" -----        -----" + "\n" +
                                @"                   " + "\n");
                            break;

                        case 17:
                            eyesStringBuilder.Append(
                                @"                   " + "\n" +
                                @" ---__        __---" + "\n" +
                                @"  ___          ___ " + "\n" +
                                @" | O |        | O |" + "\n" +
                                @" -----        -----" + "\n" +
                                @"                   " + "\n");
                            break;

                        case 18:
                            eyesStringBuilder.Append(
                                @"                   " + "\n" +
                                @" ___--        --___" + "\n" +
                                @"  ___          ___ " + "\n" +
                                @" | O |        | O |" + "\n" +
                                @" -----        -----" + "\n" +
                                @"                   " + "\n");
                            break;

                        case 20:
                            eyesStringBuilder.Append(
                                @"                   " + "\n" +
                                @" ___--        --___" + "\n" +
                                @"                   " + "\n" +
                                @" -___-        -___-" + "\n" +
                                @"                   " + "\n" +
                                @"                   " + "\n");
                            break;

                        case 21:
                            eyesStringBuilder.Append(
                                @"                   " + "\n" +
                                @" --__-        -__--" + "\n" +
                                @"  ___          ___ " + "\n" +
                                @" -   -        -   -" + "\n" +
                                @"                   " + "\n" +
                                @"                   " + "\n");
                            break;

                        case 22:
                        case 23:
                            eyesStringBuilder.Append(
                                @"                   " + "\n" +
                                @" ___--        --___" + "\n" +
                                @"  ---          --- " + "\n" +
                                @" | O |        | O |" + "\n" +
                                @" -----        -----" + "\n" +
                                @"                   " + "\n");
                            break;

                        case 25:
                            eyesStringBuilder.Append(
                                @"                   " + "\n" +
                                @" ___--        --___" + "\n" +
                                @"                   " + "\n" +
                                @" -___-        -___-" + "\n" +
                                @"                   " + "\n" +
                                @"                   " + "\n");
                            break;

                        case 26:
                            eyesStringBuilder.Append(
                                @"   ___        ___  " + "\n" +
                                @" --              --" + "\n" +
                                @"  ___          --- " + "\n" +
                                @" -   -        | O |" + "\n" +
                                @"              -----" + "\n" +
                                @"                   " + "\n");
                            break;

                        case 27:
                            eyesStringBuilder.Append(
                                @"   ___        ___  " + "\n" +
                                @" --              --" + "\n" +
                                @"  ---          ___ " + "\n" +
                                @" | O |        -   -" + "\n" +
                                @" -----             " + "\n" +
                                @"                   " + "\n");
                            break;

                        default:
                            eyesStringBuilder.Append(
                                @"                   " + "\n" +
                                @" -----        -----" + "\n" +
                                @"  ---          --- " + "\n" +
                                @" | O |        | O |" + "\n" +
                                @"  ---          --- " + "\n" +
                                @"                   " + "\n");
                            break;
                    }

                    Console.CursorLeft = 0;
                    Console.CursorTop = line;
                    Console.Write(eyesStringBuilder.ToString());

                    if (scenario.EyeClose == 1)
                    {
                        eyesStringBuilder.Clear();
                        eyesStringBuilder.Append(
                            @"                   " + "\n" +
                            @" -___-        -___-" + "\n" +
                            @"                   " + "\n" +
                            @"                   " + "\n");

                        Console.CursorLeft = 0;
                        Console.CursorTop = line + 2;
                        Console.Write(eyesStringBuilder.ToString());
                    }
                }
            }
        }
    }
}