using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformers
{
    public class ItemRecipeSO : ScriptableObject
    {
        public string recipeName;
        public RecipeType recipeType;
        public ItemTypeAndCount[] input;
        public ItemTypeAndCount[] output;

    }
    public enum RecipeType
    {
        Armour,
        Weapons
    }

    [System.Serializable]
    public class ItemTypeAndCount
    {
        public Item itemType;
        public int itemCount;

        public ItemTypeAndCount(Item itemType, int itemCount)
        {
            this.itemType = itemType;
            this.itemCount = itemCount;
        }
    }
}
