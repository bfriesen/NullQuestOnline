using System;

namespace NullQuestOnline.Game
{
    [Serializable]
    public class Character : Combatant
    {
        public override string BareHandsAttackName
        {
            get { return "Attack"; }
        }

        public void AddExperience(int experienceToAdd)
        {
            Experience += experienceToAdd;
            while (Experience > ExperienceRequiredForNextLevel(Level))
            {
                Level++;
            }
        }

        public double ProgressTowardsNextLevel()
        {
            int expIntoCurrentLevel = Experience - ExperienceRequiredForNextLevel(Level - 1);
            int additionalExperienceForNextLevel = ExperienceRequiredForNextLevel(Level) - ExperienceRequiredForNextLevel(Level - 1);
            return (double)expIntoCurrentLevel / additionalExperienceForNextLevel;
        }

        public static int ExperienceRequiredForNextLevel(int currentLevel)
        {
            if (currentLevel < 0) throw new ArgumentException("current level can't be less than 0", "currentLevel");
            if (currentLevel == 0) return 0;

            int expSum = 0;
            for (int i = 0; i <= currentLevel; i++)
            {
                expSum += (int)Math.Floor((i * 10.5) * (2 + i));
            }

            return expSum;
        }
    }
}