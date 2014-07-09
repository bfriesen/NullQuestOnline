using System;
using System.Collections.Generic;
using System.Linq;

namespace NullQuestOnline.Extensions
{
    public static class ModifierExtensions
    {
        public static int GetStatModifier(this int stat)
        {
            return Math.Max((int)Math.Floor((stat - 20) / 2.0), -1);
        }

        public static int GetLevelModifier(this int baseIncreaser, int level)
        {
            return Convert.ToInt32((level - 1) * 4 * (0.2 + (baseIncreaser / 30.0)));
        }
    }
}