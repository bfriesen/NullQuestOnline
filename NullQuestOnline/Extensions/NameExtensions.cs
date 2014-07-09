using System;
using System.Collections.Generic;
using System.Linq;
using NullQuest.Game.Combat;

namespace NullQuestOnline.Extensions
{
    public static class NameExtensions
    {
        public static string GetLeveledName(this Weapon instance)
        {
            return instance.Level > 0 ? instance.Name + " +" + instance.Level : instance.Name;
        }
    }
}