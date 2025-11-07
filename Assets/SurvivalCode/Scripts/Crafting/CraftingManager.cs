using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Platformers
{
    public class CraftingManager : MonoBehaviour
    {
        // References to UI elements and data slots
        [Header("References")]
        public Transform craftingGridParent;
        

        // Crafting recipes available
        [Header("Data")]
        public CraftingRecipeSO[] recipes;
        public InventoryManager inventoryManager;


        // holds the crafting slots
        private CraftingSlots[] slots;

        public CraftingSlots[] CraftingSlots;
        public Button Craft;
        [HideInInspector]bool allEmpty = false;
        void Start()
        {
            
            slots = craftingGridParent.GetComponentsInChildren<CraftingSlots>();
            Craft.gameObject.SetActive(false);
            CheckEmpty();


        }



        // checks every frame if the crafting grid is empty
        public void Update()
        {
            CheckEmpty();
        }
        public void CheckEmpty() {

            allEmpty = false;


            for (int i = 0; i < slots.Length; i++)
            {
                InventoryItem itemInSlot = slots[i].GetComponentInChildren<InventoryItem>();
                if (itemInSlot != null)
                {
                    allEmpty = true;
                    break;
                }
                
            }
            
            if (!allEmpty)
            {
               
                Craft.gameObject.SetActive(false);
            }
            else
            {
      
                Craft.gameObject.SetActive(true);
            }


        }
        
        // crafts the item
        public void UpdateCrafting()
        {
            // adds all the items into the current items
            Item[] currentItems = new Item[slots.Length];
            for (int i = 0; i < slots.Length; i++)
            {
                InventoryItem itemInSlot = slots[i].GetComponentInChildren<InventoryItem>();
                
                if (itemInSlot != null)
                {
                    currentItems[i] = itemInSlot.item; // Assuming itemData holds the SO
                }
            }

            


            // Finds the match
            CraftingRecipeSO matchedRecipe = FindMatchingRecipe(currentItems);
      
           
            if (matchedRecipe != null)
            {
                // adds the result and destroys all the items in the crafting slots
                // as it has been used
                inventoryManager.AddItems(matchedRecipe.result);
                foreach (CraftingSlots slot in slots)
                {
                    InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                    if (itemInSlot != null)
                    {
                       
                        Destroy(itemInSlot.gameObject);

                    }
                }

                Craft.gameObject.SetActive(false);

            }
            
        }

        // finds all the recipes and checks which recipe matches the array on the crafting grid
        private CraftingRecipeSO FindMatchingRecipe(Item[] itemsInGrid)
        {
            
            foreach (CraftingRecipeSO recipe in recipes)
            {
        
                bool isMatch = true;
                for (int i = 0; i < recipe.recipeArray.Length; i++)
                {
                    
                    if (recipe.recipeArray[i] != itemsInGrid[i])
                    {
                        isMatch = false;
                        break; 
                    }
                }

                if (isMatch)
                {
                    return recipe;
                }
            }
            // if there are no matches then return null
            return null;
        }

       
    }
}