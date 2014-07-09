using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using NullQuestOnline.Data;
using NullQuestOnline.Extensions;
using NullQuestOnline.Game;

namespace NullQuestOnline.Models
{
    public class GameWorld
    {
        private readonly IDungeonNameGenerator dungeonNameGenerator;

        public GameWorld()
        {
            dungeonNameGenerator = new DeterministicDungeonNameGenerator();
            
        }

        public static GameWorld Create(string characterName)
        {
            return new GameWorld()
            {
                Character = new Character()
                {
                    Name = characterName,
                    PowerRoll = 9 + Dice.Roll(4, 4)
                },
                CurrentDungeonLevel = 1,
            };
        }

        public Character Character { get; set; }
        [XmlAttribute] public bool Created { get; set; }
        public bool InDungeon { get; set; }
        public int CurrentDungeonLevel { get; set; }
        public int NumberOfMonstersDefeatedInCurrentDungeonLevel { get; set; }
        public int RequiredNumberOfMonstersInCurrentDungeonLevelBeforeBoss { get; set; }
        public int TotalNumberOfMonstersDefeated { get; set; }

        [XmlArrayItem("Boss")]
        public List<string> BossesDefeated { get; set; }

        public void SetRequiredNumberOfMonstersInCurrentDungeonLevelBeforeBoss()
        {
            NumberOfMonstersDefeatedInCurrentDungeonLevel = 0;
            RequiredNumberOfMonstersInCurrentDungeonLevelBeforeBoss = Character.Level + Dice.Roll(1, 6);
        }

        public string GetCurrentDungeonName()
        {
            return dungeonNameGenerator.GenerateName(CurrentDungeonLevel);
        }
    }
}