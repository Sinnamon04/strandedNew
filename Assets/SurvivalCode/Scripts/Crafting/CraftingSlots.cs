using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Platformers
{
    public class CraftingSlots : MonoBehaviour, IDropHandler
    {
        
        public Image image;

        private CharacterStatss characterStatss;

        public EquipmentSlot[] equipmentSlots;
        public BaseXPTranslations xpTranslation;
        [SerializeField] private XPTracker xpTracker;
        private void Awake()
        {
            characterStatss = GameObject.Find("Characters").GetComponent<CharacterStatss>();
        }

        // When the item is dropped into the slot chosen
        public void OnDrop(PointerEventData eventData)
        {
            bool foundEmpty = true;
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                if (equipmentSlots[i].GetComponentInChildren<InventoryItem>() != null)
                {
                    characterStatss.BaseStrength -= equipmentSlots[i].GetComponentInChildren<InventoryItem>().item.strengthBonus;
                    Debug.Log("Subtracting Strength Bonus: " + equipmentSlots[i].GetComponentInChildren<InventoryItem>().item.strengthBonus);
                    characterStatss.BaseDefense -= equipmentSlots[i].GetComponentInChildren<InventoryItem>().item.defenseBonus;
                    Debug.Log("Subtracting Defense Bonus: " + equipmentSlots[i].GetComponentInChildren<InventoryItem>().item.defenseBonus);
                    foundEmpty = false;
                }
            }

            if (foundEmpty)
            {
                if (xpTracker != null)
                {
                    characterStatss.BaseDefense = characterStatss.BaseDefensePerLevel * xpTracker.level + characterStatss.BaseDefenseOffset;
                    characterStatss.BaseStrength = characterStatss.BaseStrengthPerLevel * xpTracker.level + characterStatss.BaseStrengthOffset;
                }

                

            }

            characterStatss.StrengthText.text = $"Strength: {characterStatss.BaseStrength}";
            characterStatss.DefenseText.text = $"Defense: {characterStatss.BaseDefense}";

            if (transform.childCount == 0)
            {
                InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
                item.parentAfterDrag = transform;
            }
        }
    }
}
