using System;
using System.Collections.Generic;
using System.Linq;
using NullQuest.Game.Combat;
using NullQuestOnline.Game;

namespace NullQuestOnline.Models
{
    public class InventoryViewModel
    {
        public Weapon CurrentlyEquippedWeapon { get; set; }
        public IEnumerable<Weapon> Weapons { get; set; }

        public static InventoryViewModel Create(GameWorld gameWorld)
        {
            return new InventoryViewModel()
            {
                CurrentlyEquippedWeapon = gameWorld.Character.Weapon,
                Weapons = gameWorld.Character.Inventory
            };
        }
    }
}