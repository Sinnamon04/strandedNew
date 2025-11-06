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
        [SerializeField]private CharacterStatss characterStatss;
        public EquipmentSlot[] equipmentSlots;
        public BaseXPTranslations xpTranslation;
        [SerializeField] private XPTracker xpTracker;


        private void Awake()
        {
            Deselect();
            
            
        }

        public void Select()
        {
            image.color = selectedColor;

            
        }
     

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
                    characterStatss.BaseStrength -= equipmentSlots[i].GetComponentInChildren<InventoryItem>().item.strengthBonus;
                    Debug.Log("Subtracting Strength Bonus: " + equipmentSlots[i].GetComponentInChildren<InventoryItem>().item.strengthBonus);
                    characterStatss.BaseDefense -= equipmentSlots[i].GetComponentInChildren<InventoryItem>().item.defenseBonus;
                    Debug.Log("Subtracting Defense Bonus: " + equipmentSlots[i].GetComponentInChildren<InventoryItem>().item.defenseBonus);
                    foundEmpty = false;
                }
            }

            if (foundEmpty)
            {
                if (xpTracker != null) {

                    characterStatss.BaseDefense = characterStatss.BaseDefensePerLevel * xpTracker.level +characterStatss.BaseDefenseOffset;
                    characterStatss.BaseStrength = characterStatss.BaseStrengthPerLevel * xpTracker.level + characterStatss.BaseStrengthOffset;

                }
                Debug.Log(xpTranslation.CurrentLevel);
                

            }
           
            characterStatss.StrengthText.text = $"Strength: {characterStatss.BaseStrength}";
            characterStatss.DefenseText.text = $"Defense: {characterStatss.BaseDefense}";

            if (transform.childCount == 0) {

                InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
                item.parentAfterDrag = transform;
            }
        }
    }
}
