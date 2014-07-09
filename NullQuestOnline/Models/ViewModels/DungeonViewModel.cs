using System;
using System.Collections.Generic;
using System.Linq;

namespace NullQuestOnline.Models.ViewModels
{
    public class DungeonViewModel
    {
        public GameWorld GameWorld { get; set; }
        public string DungeonName { get; set; }
        public int DungeonLevel { get; set; }
        public string FluffText { get; set; }
        public Combatant Monster { get; set; }
    }
}