using System.Text;
using System;
using System.Linq;
using System.Xml.Serialization;
using NullQuestOnline.Extensions;
using NullQuestOnline.Game;
using NullQuestOnline.Models;

namespace NullQuest.Game.Combat
{
    [Serializable]
    public class Weapon
    {
        public static readonly Weapon BareHands =
            new Weapon
            {
                Name = "Bare Hands",
                WeaponType = WeaponType.Melee,
                Damage =
                    new Magnitude
                    {
                        BaseAmount = 0,
                        NumberOfDice = 1,
                        NumberOfSides = 4
                    }
            };

        public Weapon()
        {
        }

        public Weapon(WeaponArchetype weaponArchetype)
        {
            Damage = weaponArchetype.Damage;
            WeaponType = weaponArchetype.WeaponType;
            Name = weaponArchetype.Name;
        }

        public string Use(Combatant combatant)
        {
            if (!combatant.Weapon.Equals(BareHands))
            {
                combatant.AddItemToInventory(combatant.Weapon);
            }
            
            combatant.RemoveItemFromInventory(this);
            combatant.Weapon = this.DeepClone();
            combatant.Weapon.Quantity = 1;

            return null;
        }

        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Level { get; set; }
        public WeaponType WeaponType { get; set; }
        public Magnitude Damage { get; set; }
        public int RequiredStrength { get; set; }

        public bool Equals(Weapon otherWeapon)
        {
            if (otherWeapon == null)
            {
                return false;
            }

            return Name == otherWeapon.Name
                && WeaponType == otherWeapon.WeaponType
                && Level == otherWeapon.Level
                && Damage.Equals(otherWeapon.Damage);
        }

        public string GetDescription()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Weapon: {0}", this.GetLeveledName()).AppendLine();
            sb.AppendFormat("Damage: {0}", Damage).AppendLine();
            return sb.ToString();

        }
    }
}
