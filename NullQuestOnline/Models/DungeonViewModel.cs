using System;
using System.Collections.Generic;
using System.Linq;
using NullQuestOnline.Extensions;
using NullQuestOnline.Game;

namespace NullQuestOnline.Models
{
    public class DungeonViewModel
    {
        public string DungeonName { get; set; }
        public int DungeonLevel { get; set; }
        public string FluffText { get; set; }
        public Combatant Monster { get; set; }
        public List<string> CombatLog { get; set; }

        public static DungeonViewModel Create(GameWorld gameWorld)
        {
            return new DungeonViewModel()
            {
                DungeonName = gameWorld.GetCurrentDungeonName(),
                DungeonLevel = gameWorld.CurrentDungeonLevel,
                FluffText = 
                    "This is the dungeon - it isn't a very friendly place. Hallways go off in seemingly every direction, with no end in sight. A skeleton sits upright against a wall in the corner. No doubt an adventurer that came before you...",
                Monster = gameWorld.CurrentEncounter,
                CombatLog = gameWorld.CombatLog,
                CharacterName = gameWorld.Character.Name,
                Level = gameWorld.Character.Level,
                Gold = gameWorld.Character.Gold,
                CurrentHitPoints = gameWorld.Character.CurrentHitPoints,
                MaxHitPoints = gameWorld.Character.MaxHitPoints,
                Experience = gameWorld.Character.Experience,
                ExperienceMeter = gameWorld.Character.ProgressTowardsNextLevel(),
                CurrentWeapon = gameWorld.Character.Weapon.GetLeveledName(),
                ItemsInInventory = gameWorld.Character.Inventory.Count(),
                PlayerDead = !gameWorld.Character.IsAlive,
                MonsterDead = gameWorld.CurrentEncounter != null && !gameWorld.CurrentEncounter.IsAlive,
                PlayerFled = gameWorld.Character.HasFledCombat
            };
        }

        public string CharacterName { get; set; }
        public int Level { get; set; }
        public int Gold { get; set; }

        public int CurrentHitPoints { get; set; }
        public int MaxHitPoints { get; set; }
        public double HitPointsMeter { get { return (double)CurrentHitPoints / MaxHitPoints; } }

        public int Experience { get; set; }
        public double ExperienceMeter { get; private set; }

        public string CurrentWeapon { get; set; }
        public int ItemsInInventory { get; set; }
        public bool PlayerDead { get; set; }
        public bool MonsterDead { get; set; }
        public bool PlayerFled { get; set; }

        private static string GetMeter(double current, double max)
        {
            return GetMeter(current / max);
        }

        private static string GetMeter(double ratio)
        {
            return string.Format("{0}", new string('#', Math.Max(0, Convert.ToInt32(Math.Ceiling(ratio * 10)))).PadRight(10));
        }
    }
}