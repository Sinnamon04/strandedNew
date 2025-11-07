using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Platformers
{
    public class ItemObjects : MonoBehaviour
    {
        // class for dropped item objects in the world
        public Item item;
        public Sprite image;
        public Vector3 position;
        public InventoryManager inventoryManager;
        private Rigidbody rb;
        private Collider col;

        // initializes references to rigidbody and collider components so that physics can be applied to the dropped item
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
