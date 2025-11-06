using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Platformers
{
    public class ButtonInfo : MonoBehaviour
    {

        public int ItemID;
        public TextMeshProUGUI PriceText;
        public TextMeshProUGUI QuantityText;
        public GameObject ShopManager;
        public Item item;

       

        // Update is called once per frame
        void Update()
        {
            PriceText.text = ShopManager.GetComponent<ShopManager>().shopItems[2, ItemID].ToString();
            QuantityText.text = ShopManager.GetComponent<ShopManager>().shopItems[3, ItemID].ToString();

        }
    }
}
