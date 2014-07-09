using System;
using System.Collections.Generic;
using System.Linq;

namespace NullQuestOnline.Game
{
    public class Dice
    {
        private static readonly Random _random = new Random();
        public static int Roll(int numberOfDice, int numberOfSides)
        {
            return Roll(numberOfSides).Take(numberOfDice).Sum();
        }

        public static int Roll(Magnitude magnitude)
        {
            return magnitude.BaseAmount + Roll(magnitude.NumberOfDice, magnitude.NumberOfSides);
        }

        private static IEnumerable<int> Roll(int numberOfSides)
        {
            while (true)
            {
                yield return _random.Next(1, numberOfSides + 1);
            }
        }

        public static double Random()
        {
            return _random.NextDouble();
        }

        public static int Random(int min, int max)
        {
            return _random.Next(min, max + 1);
        }
    }
}