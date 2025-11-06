using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Platformers
{
    public class ItemObjects : MonoBehaviour
    {
        
        public Item item;
        public Sprite image;
        public Vector3 position;
        public InventoryManager inventoryManager;
        private Rigidbody rb;
        private Collider col;
        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>(); 

        }

        // sets the item data for the dropped item object
        public void SetItem(Item newItem,Vector3 positions)
        {
            item = newItem;
            image = item.image;
            position = positions;
            
        }

    }
}
