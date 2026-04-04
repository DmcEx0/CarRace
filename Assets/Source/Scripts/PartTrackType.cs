using System;

namespace CarRace
{
    [Flags]
    public enum PartTrackType
    {
        None    = 0,
        Forward = 1 << 0,
        Left    = 1 << 1,
        Right   = 1 << 2
    }
}