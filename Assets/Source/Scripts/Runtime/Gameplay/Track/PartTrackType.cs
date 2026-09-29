using System;

namespace CarRace.Gameplay.Track
{
    [Flags]
    public enum PartTrackType
    {
        None = 0,
        Forward = 1 << 0,
        Left = 1 << 1,
        Right = 1 << 2,
        ForwardUp1 = 1 << 3,
        ForwardUp2 = 1 << 4,
        ZigZag = 1 << 5,
        Circle = 1 << 6,
        Jump = 1 << 7,
    }
}