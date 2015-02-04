using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VEGA.Framework.Bot.Utility
{
    public class Formula
    {
        public static float GetPercentage(ulong val1, ulong val2)
        {
            return (float)val1 / ((float)val2 / 100f);
        }
        public static float GetXPos(byte xSec, float xOff)
        {
            return ((xSec - 135) * 192 + (xOff / 10));
        }
        public static float GetYPos(byte ySec,float yOff)
        {
            return ((ySec - 92) * 192 + (yOff / 10));
        }
    }
}
