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
                combatLog.Add(string.Format("^C{0}^N hits ^C{1}^N with ^R{2}^N for ^W{3}^N points!", attacker.Name, defender.Name, attacker.Weapon.Name, damage));
            }
            else
            {
                combatLog.Add(string.Format("^C{0}^N attempts to hit ^C{1}^N with ^R{2}^N and ^Wfails miserably^N!", attacker.Name, defender.Name, attacker.Weapon.Name));
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
                combatLog.Add(string.Format("^C{0}^N has fled the battle!", attacker.Name));
            }
            else
            {
                combatLog.Add(string.Format("^C{0}^N attempts to flee but ^C{1}^N gets in the way!", attacker.Name, defender.Name));
            }
        }
    }
}