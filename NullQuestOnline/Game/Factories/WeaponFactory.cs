using System;
using System.Collections.Generic;
using System.Linq;
using NullQuest.Game.Combat;
using NullQuestOnline.Data;

namespace NullQuestOnline.Game.Factories
{
    public class WeaponFactory
    {
        private readonly List<WeaponArchetype> weapons;

        public WeaponFactory()
        {
            IWeaponDataRepository weaponDataRepository = new HardCodedWeaponDataRepository();
            weapons = weaponDataRepository.GetAllWeapons().ToList();
        }

        public Weapon CreateWeapon(int itemLevel, bool useMaxModifier)
        {
            int minAverageDamage = 1 + (itemLevel);
            int maxAverageDamage = 2 * minAverageDamage;

            // Find all weapons with an average damage less than the max we're looking for (eventually this will be all weapons)
            var weaponCandidates = weapons.Where(x => x.Damage.GetAverageValue() < maxAverageDamage);

            // Choose a random weapon from that list
            var weaponArchetype = weaponCandidates.OrderBy(x => Dice.Random()).First();

            // Add weapon levels to put it somewhere in our min to max range.
            // First get the min and max "multipliers" for the weapon that will keep it within the range
            int minMultiplier = 0;
            int maxMultiplier = 0;

            int i = 1;
            do
            {
                if (minMultiplier == 0 && weaponArchetype.Damage.GetAverageValue() * i > minAverageDamage)
                {
                    minMultiplier = i;
                }
                if (weaponArchetype.Damage.GetAverageValue() * i > maxAverageDamage)
                {
                    maxMultiplier = i - 1;
                    break;
                }
                i++;
            } while (maxMultiplier == 0);

            // Now set the weapon's level to a multiplier in that range
            int randomMultiplier;
            if (useMaxModifier)
            {
                randomMultiplier = maxMultiplier;
            }
            else
            {
                randomMultiplier = Dice.Random(minMultiplier, maxMultiplier);
            }

            var newWeapon = new Weapon(weaponArchetype) { Level = randomMultiplier - 1, Quantity = 1, Id = Guid.NewGuid() };
            return newWeapon;
        }
    }
}