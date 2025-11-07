using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Platformers
{
    public class InventorySlot : MonoBehaviour,IDropHandler
    {
        public Image image;
        public Color selectedColor, notSelectedColor;
        [SerializeField]private CharacterStatss characterStats;
        public EquipmentSlot[] equipmentSlots;
        public BaseXPTranslations xpTranslation;
        [SerializeField] private XPTracker xpTracker;

        // initializes the slot to deselected state
        private void Awake()
        {
            Deselect();
            
            
        }
        // selects the slot and changes its color
        public void Select()
        {
            image.color = selectedColor;

            
        }

        // deselects the slot and changes its color

        public void Deselect()
        {
            image.color = notSelectedColor;
        }


        // drops item into slot and updates character stats
        public void OnDrop(PointerEventData eventData)
        {
            bool foundEmpty = true;
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                if (equipmentSlots[i].GetComponentInChildren<InventoryItem>() != null)
                {
                    characterStats.BaseStrength -= equipmentSlots[i].GetComponentInChildren<InventoryItem>().item.strengthBonus;
                    Debug.Log("Subtracting Strength Bonus: " + equipmentSlots[i].GetComponentInChildren<InventoryItem>().item.strengthBonus);
                    characterStats.BaseDefense -= equipmentSlots[i].GetComponentInChildren<InventoryItem>().item.defenseBonus;
                    Debug.Log("Subtracting Defense Bonus: " + equipmentSlots[i].GetComponentInChildren<InventoryItem>().item.defenseBonus);
                    foundEmpty = false;
                }
            }
            // if no equipped items found, recalculate base stats from level

            if (foundEmpty)
            {
                if (xpTracker != null) {

                    characterStats.BaseDefense = characterStats.BaseDefensePerLevel * xpTracker.level +characterStats.BaseDefenseOffset;
                    characterStats.BaseStrength = characterStats.BaseStrengthPerLevel * xpTracker.level + characterStats.BaseStrengthOffset;

                }
                
                

            }
            // update the character stats UI

            characterStats.StrengthText.text = $"Strength: {characterStats.BaseStrength}";
            characterStats.DefenseText.text = $"Defense: {characterStats.BaseDefense}";

            if (transform.childCount == 0) {

                InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
                item.parentAfterDrag = transform;
            }
        }
    }
}
