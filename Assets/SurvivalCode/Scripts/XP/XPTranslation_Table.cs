using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformers
{
    [System.Serializable]
    public class XPTranslationTableEntry
    {
        public int Level;
        public int XPRequired;
    }
    [CreateAssetMenu(menuName = "RPG/XP Table", fileName = "XPTranslation_Table")]

    /// XP translation system based on a predefined table of levels and required XP
    public class XPTranslation_Table : BaseXPTranslations
    {
        public List<XPTranslationTableEntry> Table;
        private bool AtLevelCap;
        // Adds XP and handles level up logic based on the predefined table
        public override bool AddXP(int amount)
        {
            if (IsMaxLevel)
                return false;
            CurrentXP += amount;
            for (int index = Table.Count - 1; index >= 0; index--)
            {
                var entry = Table[index];
                if (CurrentXP >= entry.XPRequired)
                {
                    CurrentLevel = entry.Level;
                    AtLevelCap = Table[Table.Count - 1].Level == CurrentLevel;
                    return true;
                }
            }
            return false;
            
        }
        // Sets the current level directly based on the predefined table
        public override void SetLevel(int level)
        {
            CurrentLevel = 1;
            CurrentXP = 0;
            IsMaxLevel = false;
            foreach (var entry in Table)
            {
                if (entry.Level == level)
                {
                    AddXP(entry.XPRequired);
                    return;
                }
            }
            throw new System.ArgumentOutOfRangeException("level", "Level not found in XP table");
        }

        // Calculates the XP required to reach the next level based on the predefined table
        protected override int GetXRequiredForNextLevel()
        {
            if (IsMaxLevel)
                return int.MaxValue;

            for (int i = 0; i < Table.Count; i++)
            {
                if (Table[i].Level == CurrentLevel + 1)
                {
                    return Table[i + 1].XPRequired - CurrentXP;
                }
            }
            throw new System.ArgumentOutOfRangeException("level","Current level not found");
        }

  
       
    }
}
