using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



namespace Platformers
{

    // EquipmentSlot handles the UI slot where equipment items can be dropped and equipped, it also updates the character stats accordingly.
    public class EquipmentSlot : MonoBehaviour, IDropHandler
    {
        
            
        [SerializeField] private EquipmentType slotType;

        private EquipmentManager equipmentManager;

        void Awake()
        {

            equipmentManager = Object.FindFirstObjectByType<EquipmentManager>();
            if (equipmentManager == null)
            {
                Debug.LogError("EquipmentSlot could not find an EquipmentManager in the scene!");
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            
            InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();

            
            if (transform.childCount > 0)
            {
              
                return;
            }



            
            if (item.item.equipmentType == this.slotType)
            {

                item.transform.SetParent(this.transform);
                item.transform.localPosition = Vector3.zero; 

                item.parentAfterDrag = this.transform;

                UpdateEquipmentStats();
            }
            
        }

        public void UpdateEquipmentStats()
        {
            // We use a small delay to ensure the UI parenting has finished before we calculate.
            StartCoroutine(DelayedStatUpdate());
        }
        // Coroutine to delay the stat update until the end of the frame to ensure UI changes are applied.
        private System.Collections.IEnumerator DelayedStatUpdate()
        {
            
            yield return new WaitForEndOfFrame();

            if (equipmentManager != null)
            {
                equipmentManager.RecalculateAllStats();
            }
        }
    }

    // Enum defining the different types of equipment slots available.
    public enum EquipmentType
    {
        None, 
        Helmet,
        ChestPlate,
        Leggings,
        Boots,
        RightHand,
        LeftHand
    }
}