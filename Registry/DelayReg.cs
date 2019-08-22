using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MusicBuilder.Registry
{

    public static class DelayReg
    {
        public static Dictionary<ushort, DelayData> delayData;

        public class DelayData
        {
            public readonly Color bgc;
            public readonly Color off;
            public readonly Color lit;

            public DelayData(uint bgc, uint off, uint lit)
            {
                this.bgc.PackedValue = bgc;
                this.off.PackedValue = off;
                this.lit.PackedValue = lit;
            }
        }
    }
}
