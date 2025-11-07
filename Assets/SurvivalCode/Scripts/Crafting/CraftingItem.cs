using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Platformers
{
    public class CraftingItem : MonoBehaviour
    {
        
        [Header("UI")]
        public Image image;
        public TextMeshProUGUI countText;
        public InventoryManager inventoryManager;

        // Drag and Drop Variables
        [HideInInspector] public Transform parentAfterDrag;
        [HideInInspector] public int count = 1;
        [HideInInspector] public Item item;


        // count is refreshed whenever it changes
        public void RefreshCount()
        {
            countText.text = count.ToString();
            bool isActive = count > 1;
            countText.gameObject.SetActive(isActive);
        }


        // Drag and Drop Functionality the item allows itself to be dragged around the crafting table
        public void OnBeginDrag(PointerEventData eventData)
        {
            image.raycastTarget = false;
            parentAfterDrag = transform.parent;

            transform.SetParent(transform.root);

        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            image.raycastTarget = true;

            transform.SetParent(parentAfterDrag);



        }

    }
}
