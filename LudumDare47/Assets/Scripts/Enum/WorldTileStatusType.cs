using System;

namespace Enum
{
    [Flags]
    public enum WorldTileStatusType
    {
        None = 0,
        Invalid = 1 << 0,
        NotInitialized = 1 << 1,
        Blocked = 1 << 2,
        Buildable = 1 << 3,
        Upgradeable = 1 << 4
    }
}