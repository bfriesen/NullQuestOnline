using System;
using System.Collections.Generic;
using System.Linq;

namespace NullQuestOnline.Models.ViewModels
{
    public class DungeonViewModel
    {
        public string DungeonName { get; set; }
        public int DungeonLevel { get; set; }
        public string FluffText { get; set; }
        public Combatant Monster { get; set; }

        public static DungeonViewModel Create(GameWorld gameWorld)
        {
            return new DungeonViewModel()
            {
                DungeonName = gameWorld.GetCurrentDungeonName(),
                DungeonLevel = gameWorld.CurrentDungeonLevel,
                FluffText = 
                    "This is the dungeon - it isn't a very friendly place. Hallways go off in seemingly every direction, with no end in sight. A skeleton sits upright against a wall in the corner. No doubt an adventurer that came before you...",
                Monster = gameWorld.CurrentEncounter
            };
        }
    }
}