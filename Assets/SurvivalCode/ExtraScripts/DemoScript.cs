using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Platformers
{
    public class DemoScript : MonoBehaviour, IPointerClickHandler
    {
        // Start is called before the first frame update
        public InventoryManager inventoryManager;
        public CraftingManager craftingManager;
        public Item[] items;

        public void OnClick(PointerEventData eventData)
        {
            Debug.Log("Clicked on item: ");
        }


        public void pickUpItems(int id) {

            Debug.Log(id);


            bool result = inventoryManager.AddItems(items[id]);
            if (result)
            {
                Debug.Log("Item added");
            }
            else {
                Debug.Log("Item not added");
            }
        
        }

        public void runsThing() {
            craftingManager.UpdateCrafting();
        }

        public void GetSelectedItem() {
            Item receivedItem = inventoryManager.GetSelectedItem(false);
            if (receivedItem != null)
            {
                Debug.Log("received Item");
            }
            else {
                Debug.Log("greger");
            }
        }

        public void UseSelectedItem() {

            Item receivedItem = inventoryManager.GetSelectedItem(true);
            if (receivedItem != null)
            {
                Debug.Log("used Item");
            }
            else
            {
                Debug.Log("not used item");
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Clicked on item: ");
        }
    }
}
