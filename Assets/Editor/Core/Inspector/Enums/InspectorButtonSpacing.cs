using System;

namespace Core
{
    [Flags]
    public enum InspectorButtonSpacing
    {
        // Decimal          // Binary
        None        = 0,
        Before      = 1,    // 000001
        After       = 2,    // 000010

        Both        = Before | After    // 000011
    }
}
