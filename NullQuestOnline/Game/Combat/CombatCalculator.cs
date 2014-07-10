using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NullQuestOnline.Extensions;

namespace NullQuestOnline.Game.Combat
{
    public class CombatCalculator
    {
        public static void Attack(Combatant attacker, Combatant defender, List<string> combatLog)
        {
            int attack = attacker.PowerRoll;
            int defend = defender.PowerRoll;

            var toHitThreshold = ((attack + ((attacker.Level - defender.Level) / 2)) / ((double)attack + defend)).ConstrainWithinBounds(0.20, 0.80);
            Debug.WriteLine("{0} has a {1:P0} chance to hit {2}", attacker.Name, toHitThreshold, defender.Name);

            if (Dice.Random() < toHitThreshold)
            {
                var damage = Math.Max(1, attacker.GetAttackDamage());
                defender.LowerHealth(damage);
                combatLog.Add(string.Format("{0} hits {1} with {2} for {3} points!", attacker.Name, defender.Name, attacker.Weapon.Name, damage));
            }
            else
            {
                combatLog.Add(string.Format("{0} attempts to hit {1} with {2} and fails miserably!", attacker.Name, defender.Name, attacker.Weapon.Name));
            }
        }

        public static void Flee(Combatant attacker, Combatant defender, List<string> combatLog)
        {
            int attackerFleeRating = Math.Max(1, attacker.Level + attacker.PowerRoll.GetStatModifier());
            int defenderFleeRating = Math.Max(1, defender.Level + defender.PowerRoll.GetStatModifier());

            var toFleeThreshold = ((attackerFleeRating) / ((double)attackerFleeRating + defenderFleeRating)).ConstrainWithinBounds(0.20, 0.80);
            Debug.WriteLine("{0} has a {1:P0} chance to flee from {2}", attacker.Name, toFleeThreshold, defender.Name);

            if (Dice.Random() < toFleeThreshold)
            {
                attacker.HasFledCombat = true;
                combatLog.Add(string.Format("{0} has fled the battle!", attacker.Name));
            }
            else
            {
                combatLog.Add(string.Format("{0} attempts to flee but {1} gets in the way!", attacker.Name, defender.Name));
            }
        }
    }
}