using System;
using System.Xml.Serialization;
using NullQuestOnline.Extensions;

namespace NullQuestOnline.Models
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

        public abstract string BareHandsAttackName { get; }

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
    }
}