using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NullQuest.Game.Combat;
using NullQuestOnline.Extensions;

namespace NullQuestOnline.Game
{
    public abstract class Combatant
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public int Level { get; set; }

        [XmlAttribute]
        public int Gold { get; set; }

        [XmlAttribute]
        public int Experience { get; set; }

        [XmlAttribute]
        public int PowerRoll { get; set; }

        [XmlAttribute]
        public int CurrentHitPoints { get; set; }

        public int MaxHitPoints { get { return (int)Math.Round((double)(25 + (Level * (10 + PowerRoll.GetStatModifier()))), MidpointRounding.AwayFromZero); } }

        public void RestoreHealth(int healthToRestore)
        {
            CurrentHitPoints = Math.Min(MaxHitPoints, CurrentHitPoints + healthToRestore);
        }

        public void LowerHealth(int healthToLower)
        {
            CurrentHitPoints = Math.Max(CurrentHitPoints - healthToLower, 0);
        }

        public bool IsAlive
        {
            get { return CurrentHitPoints > 0; }
        }

        protected readonly IList<Weapon> _inventory = new List<Weapon>();
        public IEnumerable<Weapon> Inventory { get { return _inventory; } }

        private Weapon _weapon;
        public Weapon Weapon
        {
            get { return _weapon ?? Weapon.BareHands; }
            set { _weapon = value; }
        }
        public abstract string BareHandsAttackName { get; }

        public int GetAttackDamage()
        {
            return PowerRoll.GetStatModifier() + (Dice.Roll(Weapon.Damage) * (Weapon.Level + 1));
        }

        public bool HasFledCombat { get; set; }
        public string GetDescription()
        {
            double hpPercent = (double)CurrentHitPoints / MaxHitPoints;
            if (CurrentHitPoints <= 0)
                return "dead";
            if (hpPercent < 0.10)
                return "nearly dead";
            if (hpPercent < 0.25)
                return "severely wounded";
            if (hpPercent < 0.40)
                return "badly injured";
            if (hpPercent < 0.50)
                return "seriously damaged";
            if (hpPercent < 0.65)
                return "noticeably hurt";
            if (hpPercent < 0.80)
                return "somewhat bruised";
            if (hpPercent < 0.90)
                return "barely scratched";
            return "healthy";
        }

        public void ClearInventory()
        {
            _inventory.Clear();
        }

        public void AddItemToInventory(Weapon item)
        {
            var existingItem = _inventory.SingleOrDefault(x => x.Equals(item));
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                if (item.Quantity == 0)
                {
                    item.Quantity = 1;
                }

                _inventory.Add(item);
            }
        }

        public void RemoveItemFromInventory(Weapon item)
        {
            var existingItem = _inventory.SingleOrDefault(x => x.Equals(item));
            if (existingItem != null)
            {
                existingItem.Quantity--;
                if (existingItem.Quantity == 0)
                {
                    _inventory.Remove(existingItem);
                }
            }
        }

        public void MoveItemToTopOfInventory(Weapon item, int currentIndex)
        {
            _inventory.RemoveAt(currentIndex);
            _inventory.Insert(0, item);
        }
    }
}