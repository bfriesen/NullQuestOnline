using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NullQuestOnline.Game;

namespace NullQuest.Game.Combat
{
    public class WeaponArchetype
    {
        public string Name { get; set; }
        public Magnitude Damage { get; set; }
        public WeaponType WeaponType { get; set; }

        public WeaponArchetype(string name)
        {
            Name = name;
        }
    }
}
