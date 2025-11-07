using UnityEngine;

namespace Platformers
{
    public class EquipmentManager : MonoBehaviour
    {
        // References to equipment slots and player stats
        [SerializeField] private EquipmentSlot[] equipmentSlots; 
        [SerializeField] private FirstPersonController playerController; 
        [SerializeField] private CharacterStatss characterStats; 
        void Start()
        {
            // Initial calculation of stats based on equipped items

            if (equipmentSlots.Length != 0) {
                RecalculateAllStats();
            }
        }

        // Recalculate stats based on the stats of the equipped items
        public void RecalculateAllStats()
        {
            int totalArmorBonus = 0;
            int totalDamageBonus = 0;

            // Loop through each equipment slot and gets the total bonuses
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
             
                    InventoryItem equippedItemUI = equipmentSlots[i].GetComponentInChildren<InventoryItem>();
                 

                    if (equippedItemUI != null)
                    {
                        // Get the actual data from the item
                        Item itemData = equippedItemUI.item;
                        if (itemData != null)
                        {
                            // Add this item's stats to our totals
                            totalArmorBonus += itemData.defenseBonus;
                            totalDamageBonus += itemData.strengthBonus; // Assuming strength adds to damage
                        }
                    }
                
            }

            // Apply the total bonuses to the player's stats
            if (playerController != null)
            {
                characterStats.BaseDefense = characterStats.BaseDefense + totalArmorBonus;
                characterStats.BaseStrength = characterStats.BaseStrength + totalDamageBonus;
                characterStats.StrengthText.text = $"Strength: {characterStats.BaseStrength}";
                characterStats.DefenseText.text = $"Defense: {characterStats.BaseDefense}";
                

               
            }
        }
    }
}