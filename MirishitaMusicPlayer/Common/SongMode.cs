using System;

namespace MirishitaMusicPlayer.Common
{
    [Flags]
    public enum SongMode
    {
        Normal = 0x1,
        Utaiwake = 0x2,
        OngenSentaku = 0x4,
        Instrumental = 0x8
    }
}
