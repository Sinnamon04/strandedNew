using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Platformers
{

    [CreateAssetMenu(menuName = "RPG/XP Linear", fileName = "XPTranslation_Linear")]
    public class XPTranslation_Linear : BaseXPTranslations
    {
        [SerializeField] int Offset = 100;
        [SerializeField] int Slope = 50;
        [SerializeField] int LevelCap = 20;

        // Calculates the total XP required to reach a specific level
        protected int XPForLevel(int level)
        {
            return Mathf.FloorToInt((Mathf.Min(LevelCap,level) - 1) * Slope + Offset);
        }

        // Adds XP and handles level up logic and checks for max level and if leveled up
        public override bool AddXP(int amount)
        {
            if (IsMaxLevel)
                return false;
            CurrentXP += amount;
            int newLevel = Mathf.Min(Mathf.FloorToInt((CurrentXP - Offset) / Slope) + 1,LevelCap);
            bool leveledUp = newLevel != CurrentLevel;

            CurrentLevel = newLevel;
            IsMaxLevel = CurrentLevel == LevelCap;
            return leveledUp;

        }
        // Sets the current level directly and adjusts XP accordingly
        public override void SetLevel(int level)
        {
            CurrentLevel = 1;
            CurrentXP = 0;
            IsMaxLevel = false;
            AddXP(XPForLevel(level));
           
        }
        // Calculates the XP required to reach the next level
        protected override int GetXRequiredForNextLevel()
        {
            if (IsMaxLevel)
                return int.MaxValue;

            return XPForLevel(CurrentLevel + 1) - CurrentXP;
         
        }

        
    }
}
