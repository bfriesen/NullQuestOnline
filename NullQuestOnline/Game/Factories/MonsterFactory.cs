using System;
using System.Collections.Generic;
using System.Linq;
using NullQuestOnline.Data;
using NullQuestOnline.Extensions;
using NullQuestOnline.Game.Combat;

namespace NullQuestOnline.Game.Factories
{
    public class MonsterFactory
    {
        private readonly List<MonsterArchetype> monsterArchetypes = new List<MonsterArchetype>();
        private readonly List<MonsterModifier> monsterModifiers = new List<MonsterModifier>();
        private readonly WeaponFactory weaponFactory;

        public MonsterFactory()
        {
            IMonsterDataRepository monsterDataRepository = new HardCodedMonsterDataRepository();
            weaponFactory = new WeaponFactory();
            monsterArchetypes = monsterDataRepository.GetAllMonsterArchetypes().ToList();
            monsterModifiers = monsterDataRepository.GetAllMonsterModifiers().ToList();
        }

        public Monster CreateMonster(GameWorld gameWorld)
        {
            bool isBoss = gameWorld.NumberOfMonstersDefeatedInCurrentDungeonLevel >= gameWorld.RequiredNumberOfMonstersInCurrentDungeonLevelBeforeBoss;
            var monster = CreateMonster(gameWorld.CurrentDungeonLevel, isBoss);
            //Debug.WriteLine("Created {0} (Level {1}) HP:{6} ATK:{2} DEF:{3}. Player ATK:{4} DEF:{5}", monster.Name, monster.Level, monster.ToHitAttack, monster.ToHitDefense, combatContext.Player.ToHitAttack, combatContext.Player.ToHitDefense, monster.MaxHitPoints);
            //if (monster.Weapon != null) Debug.WriteLine("Monster is wielding a {0} ({1})", monster.Weapon.GetLeveledName(), monster.Weapon.Damage);
            return monster;
        }

        public Monster CreateMonster(int dungeonLevel, bool boss)
        {
            Monster newMonster;
            if (boss)
            {
                int bossLevel = dungeonLevel.GetBossLevel();
                newMonster = CreateArchetypedMonster(bossLevel, bossLevel);
                newMonster.Name = newMonster.Name.ToUpper();
                newMonster.IsBoss = true;
            }
            else
            {
                int minLevel = Convert.ToInt32((dungeonLevel * 1.2) / 2);
                int maxLevel = dungeonLevel;

                newMonster = CreateArchetypedMonster(minLevel, maxLevel);
            }

            return AssignMonsterStats(newMonster);
        }

        private Monster AssignMonsterStats(Monster monster)
        {
            monster.CurrentHitPoints = monster.MaxHitPoints;

            var bossMultiplier = monster.IsBoss ? 6 : 1;
            monster.PowerRoll = monster.IsBoss ? 17 + Dice.Roll(2, 4) : 12 + Dice.Roll(1, 6);
            monster.CurrentHitPoints = monster.MaxHitPoints;
            monster.Gold = monster.Level * Dice.Roll(3 * bossMultiplier, 10);
            monster.Experience = (monster.Level * Dice.Roll(2 * bossMultiplier, 8));

            if (monster.IsBoss)
            {
                monster.Weapon = weaponFactory.CreateWeapon(monster.Level + 1, true);
            }
            else
            {
                if (Dice.Random(1, 4) == 1)
                {  // 1/4 monsters will have a weapon
                    monster.Weapon = weaponFactory.CreateWeapon(monster.Level, false);
                }
            }

            return monster;
        }

        private Monster CreateArchetypedMonster(int monsterMinLevel, int monsterMaxLevel)
        {
            var newMonster = new Monster();

            var targetMonsterLevel = Dice.Random(monsterMinLevel, monsterMaxLevel);

            var modifierMinimum = 0;
            var modifierMaximum = 0;

            bool targetMonsterHasModifier = Dice.Random(1, 3) == 1; // 1/3 chance of having a modifier

            if (targetMonsterHasModifier)
            {
                modifierMinimum = monsterModifiers.Select(x => x.LevelModifier.GetMinimumValue()).OrderBy(x => x).First();
                modifierMaximum = monsterModifiers.Select(x => x.LevelModifier.GetMaximumValue()).OrderByDescending(x => x).First();
            }

            int targetMonsterMaxLevel = Math.Max(1, targetMonsterLevel - modifierMinimum);
            int targetMonsterMinLevel = Math.Max(1, targetMonsterLevel - modifierMaximum);

            var archetype = ChooseArchetypeInLevelRange(targetMonsterMinLevel, targetMonsterMaxLevel);

            // We now have the archetype. Figure out what level it's going to be
            int monsterLevel = Dice.Random(archetype.Level.MinValue, archetype.Level.MaxValue);

            if (targetMonsterHasModifier)
            {
                // If we're going to have a modifier, start applying random modifiers until we get a monster in our level range.
                var modifier = ChooseModifierInLevelRange(monsterMinLevel - monsterLevel, monsterMaxLevel - monsterLevel);
                monsterLevel += Dice.Roll(modifier.LevelModifier);
                newMonster.Name = modifier.Prefix + " " + archetype.Name;
            }
            else
            {
                // Otherwise, force the monster to be within our level range if the randomly generated level is out of range.
                newMonster.Name = archetype.Name;
            }

            newMonster.Level = Math.Max(Math.Min(monsterLevel, monsterMaxLevel), monsterMinLevel);

            newMonster.SetBareHandsAttackName(archetype.AttackName);

            return newMonster;
        }

        private MonsterModifier ChooseModifierInLevelRange(int minLevel, int maxLevel)
        {
            var randomMatchingModifier = monsterModifiers.Where(x => Overlap(minLevel, maxLevel, x.LevelModifier.GetMinimumValue(), x.LevelModifier.GetMaximumValue())).OrderBy(x => Dice.Random()).FirstOrDefault();

            if (randomMatchingModifier == null)
            {
                // Didn't find any. Either our minlevel is too high, or our maxlevel is too low.
                if (monsterArchetypes.All(x => x.Level.MaxValue < minLevel))
                {
                    // If all the monsters are too low, return the highest-level one
                    return monsterModifiers.OrderByDescending(x => x.LevelModifier.GetMaximumValue()).First();
                }
                else
                {
                    // Otherwise return the lowest-level one
                    return monsterModifiers.OrderBy(x => x.LevelModifier.GetMinimumValue()).First();
                }
            }
            return randomMatchingModifier;
        }

        private MonsterArchetype ChooseArchetypeInLevelRange(int minLevel, int maxLevel)
        {
            var matchingArchetypes = monsterArchetypes.Where(x => Overlap(minLevel, maxLevel, x.Level.MinValue, x.Level.MaxValue));
            var randomMatchingArchetype = matchingArchetypes.OrderBy(x => Dice.Random()).FirstOrDefault();

            if (randomMatchingArchetype == null)
            {
                // Didn't find any. Either our minlevel is too high, or our maxlevel is too low.
                if (monsterArchetypes.All(x => x.Level.MaxValue < minLevel))
                {
                    // If all the monsters are too low, return the highest-level one
                    return monsterArchetypes.OrderByDescending(x => x.Level.MaxValue).First();
                }
                else
                {
                    // Otherwise return the lowest-level one
                    return monsterArchetypes.OrderBy(x => x.Level.MinValue).First();
                }
            }
            return randomMatchingArchetype;
        }

        private bool Overlap(int minLevel, int maxLevel, int levelMinValue, int levelMaxValue)
        {
            if (minLevel >= levelMinValue && minLevel <= levelMaxValue)
                return true;
            if (levelMinValue >= minLevel && levelMinValue <= maxLevel)
                return true;
            return false;
        }
    }
}