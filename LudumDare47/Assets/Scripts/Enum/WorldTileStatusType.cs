using System;

namespace Enum
{
    [Flags]
    public enum WorldTileStatusType
    {
        Invalid = 0,
        NotInitialized = 1,
        Blocked = 2,
        CanBeBuiltOn = 4,
        CanBeUpgraded = 6
    }
}