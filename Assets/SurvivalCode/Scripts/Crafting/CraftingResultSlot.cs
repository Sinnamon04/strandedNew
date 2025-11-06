using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformers
{

    public class CraftingResultSlot : MonoBehaviour
    {

        public GameObject itemPrefab;
        
        // displays the item after the crafting is done
        public void DisplayItem(Item item)
        {
            GameObject itemObj = Instantiate(itemPrefab, transform);

            itemObj.transform.SetParent(transform);

            InventoryItem item1 = itemObj.GetComponent<InventoryItem>();
            item1.InitialiseItem(item);

        }

        // clears the crafted item
        public void ClearSlot()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
