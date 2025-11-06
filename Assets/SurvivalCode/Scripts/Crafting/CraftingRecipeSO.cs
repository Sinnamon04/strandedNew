using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformers
{
    // the recipe of an item
    [CreateAssetMenu(fileName = "Recipe", menuName = "Inventory/Recipe")]
    public class CraftingRecipeSO : ScriptableObject
    {
        public Item result;

        public Item[] recipeArray;

    }
}
