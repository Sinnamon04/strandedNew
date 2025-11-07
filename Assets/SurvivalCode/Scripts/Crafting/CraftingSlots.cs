using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Platformers
{
    public class CraftingSlots : MonoBehaviour, IDropHandler
    {

        // UI Elements

        public Image image;

        private CharacterStatss characterStats;

        public EquipmentSlot[] equipmentSlots;
        public BaseXPTranslations xpTranslation;
        [SerializeField] private XPTracker xpTracker;
        private void Awake()
        {
            characterStats = GameObject.Find("Characters").GetComponent<CharacterStatss>();
        }

        // When the item is dropped into the slot chosen
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
                if (xpTracker != null)
                {
                    characterStats.BaseDefense = characterStats.BaseDefensePerLevel * xpTracker.level + characterStats.BaseDefenseOffset;
                    characterStats.BaseStrength = characterStats.BaseStrengthPerLevel * xpTracker.level + characterStats.BaseStrengthOffset;
                }

                
            }

            characterStats.StrengthText.text = $"Strength: {characterStats.BaseStrength}";
            characterStats.DefenseText.text = $"Defense: {characterStats.BaseDefense}";

            if (transform.childCount == 0)
            {
                InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
                item.parentAfterDrag = transform;
            }
        }
    }
}
