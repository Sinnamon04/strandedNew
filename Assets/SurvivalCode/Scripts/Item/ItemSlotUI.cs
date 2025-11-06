//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.UI;
//namespace Platformers
//{
//    public class ItemSlotUI : MonoBehaviour
//    {
//        // Assign these child objects in the prefab's Inspector
//        public Image itemIcon;
//        public Text itemInfo;
//        public Button buyButton;

//        private Item currentItem;

//        // This method is called by the TradeUIManager to setup the slot.
//        public void SetItem(Item item)
//        {
//            currentItem = item;
//            if (currentItem == null) return;

//            if (itemIcon != null) itemIcon.sprite = currentItem.image;
//            if (itemInfo != null) itemInfo.text = currentItem.itemName + " - " + currentItem.price + "g";
//        }
//    }
//}


